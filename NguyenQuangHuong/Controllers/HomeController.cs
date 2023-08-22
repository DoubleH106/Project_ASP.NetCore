using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenQuangHuong.Models;
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NguyenQuangHuong.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly Project3Nhom2Context _project3Nhom2Context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public HomeController(ILogger<HomeController> logger, Project3Nhom2Context project3Nhom2Context, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_project3Nhom2Context = project3Nhom2Context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			List<Business> Business = _project3Nhom2Context.Businesses.ToList();
			List<Employee> employees = _project3Nhom2Context.Employees.Include(m => m.Department).Include(m => m.Branch).Where(e => e.RoleId == null)
												.OrderByDescending(e => e.CreateDate)
												.Take(3)
												.ToList();
			ViewBag.employees = employees;
			ViewData["listchat"] = _project3Nhom2Context.Chats.ToList();
			List<ServiceBooking> ervicessBookings = _project3Nhom2Context.ServiceBookings.Where(m => m.Confirm == false && m.Status == false).ToList();
			var time = DateTime.Now;
			foreach (var item in ervicessBookings)
			{
				if (item.EndBookingDate <= time)
				{
					var service = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == item.ServiceId).FirstOrDefault();
					if (service != null)
					{
						item.Status = true;
						_project3Nhom2Context.ServiceBookings.Update(item);
						await _project3Nhom2Context.SaveChangesAsync();

						service.Numberofprople += item.NumberOfGuards;
						_project3Nhom2Context.Servicesses.Update(service);
						await _project3Nhom2Context.SaveChangesAsync();
					}
				}
			}
			return View(Business);
		}

		public IActionResult BaoveUser(int? id, int? serviceID)
		{
			var baove = _project3Nhom2Context.Servicesses.ToList();
			var ServiceID = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == serviceID).ToList();

			ViewBag.huong = serviceID;

			ViewBag.baove = ServiceID;
			return View(baove);
		}

		public IActionResult Chitiet(int? id)
		{
			var chitiet = _project3Nhom2Context.Servicesses.Include(m => m.Testimonials).ThenInclude(m => m.AccountsUser).FirstOrDefault(m => m.ServiceId == id);
			return View(chitiet);
		}
		// GET: Admin/ServiceBookings/Delete/5
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_project3Nhom2Context.ServiceBookings == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.ServiceBookings'  is null.");
			}
			var serviceBooking = await _project3Nhom2Context.ServiceBookings.FindAsync(id);
			if (serviceBooking != null)
			{
				Servicess servicess = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == serviceBooking.ServiceId).FirstOrDefault();
				Order order = _project3Nhom2Context.Orders.Where(m => m.Id == serviceBooking.OrderId).FirstOrDefault();

				servicess.Numberofprople += serviceBooking.NumberOfGuards;
				_project3Nhom2Context.ServiceBookings.Remove(serviceBooking);
				_project3Nhom2Context.Orders.Remove(order);
			}

			await _project3Nhom2Context.SaveChangesAsync();
			return RedirectToAction(nameof(order));
		}
		[HttpGet]
		public IActionResult Duyet()
		{
			var permissionClain = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "idusser");
			if (permissionClain != null && int.TryParse(permissionClain.Value, out int idusser))
			{
				if (idusser != null)
				{
					var order = _project3Nhom2Context.ServiceBookings.Include(m => m.Service).Include(m => m.Order).Where(m => m.Confirm == true && m.Status == false && m.Order.AccountId == idusser);
					//if (order.)
					//{

					//}
					return View(order);
				}
			}
			return View();
		}

		public IActionResult order()
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				if (idusser != null)
				{
					var order = _project3Nhom2Context.ServiceBookings.Include(m => m.Service).Include(m => m.Order).Where(m => m.Confirm == false && m.Status == false && m.Order.AccountId == idusser);
					return View(order);
				}
				else
				{
					TempData["ErrorMessage"] = "Bạn chưa đặt dịch vụ nào";
					return View();
				}
			}
			return View();
		}

		// GET: Admin/ServiceBookings/Edit/5
		public async Task<IActionResult> EditOrder(int? id)
		{
			if (id == null || _project3Nhom2Context.ServiceBookings == null)
			{
				return NotFound();
			}

			var serviceBooking = await _project3Nhom2Context.ServiceBookings.FindAsync(id);
			if (serviceBooking == null)
			{
				return NotFound();
			}
			ViewData["OrderId"] = new SelectList(_project3Nhom2Context.Orders, "Id", "Id", serviceBooking.OrderId);
			ViewData["ServiceId"] = new SelectList(_project3Nhom2Context.Servicesses, "ServiceId", "ServiceName", serviceBooking.ServiceId);
			return View(serviceBooking);
		}

		// POST: Admin/ServiceBookings/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditOrder(int id, int numberCur, [Bind("BookingId,ServiceId,BookingDate,EndBookingDate,BookingStatus,Status,Confirm,Price,NumberOfGuards,OrderId")] ServiceBooking serviceBooking)
		{
			if (id != serviceBooking.BookingId)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var number = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == serviceBooking.ServiceId).FirstOrDefault();


					if (number != null)
					{
						number.Numberofprople += numberCur;

						number.Numberofprople -= serviceBooking.NumberOfGuards;
					}
					_project3Nhom2Context.Update(number);
					await _project3Nhom2Context.SaveChangesAsync();
					//serviceBooking.BookingStatus = true;
					//serviceBooking.Confirm = false;
					//serviceBooking.Status = false;
					_project3Nhom2Context.Update(serviceBooking);
					await _project3Nhom2Context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{

				}
				return RedirectToAction(nameof(order));
			}
			ViewData["OrderId"] = new SelectList(_project3Nhom2Context.Orders, "Id", "Id", serviceBooking.OrderId);
			ViewData["ServiceId"] = new SelectList(_project3Nhom2Context.Servicesses, "ServiceId", "ServiceId", serviceBooking.ServiceId);
			return View(serviceBooking);
		}


		[HttpGet]
		public async Task<IActionResult> Datngay(int? id)
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			ViewBag.idbusiness = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == id).FirstOrDefault().BusinesId;
			var package = _project3Nhom2Context.Servicesses.Where(m => m.ServiceId == id).FirstOrDefault();
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				if (idusser != null)
				{
					ViewBag.id = id;
					ViewBag.package = package.Package;
					ViewBag.price = package.ServicePrice;
					return View();
				}
				else
				{
					return RedirectToAction("DangNhap", "Home");
				}
			}
			return RedirectToAction("DangNhap", "Home");
		}
		[HttpPost]
		public async Task<IActionResult> Datngay([Bind("BookingDate,EndBookingDate,Price,NumberOfGuards")] ServiceBooking serviceBooking, int? idServicess, string price, int package)
		{
			var numberofpeopel = _project3Nhom2Context.Servicesses.FirstOrDefault(m => m.ServiceId == idServicess);
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				//if (numberofpeopel.Numberofprople >= serviceBooking.NumberOfGuards)
				//{
				var acc = _project3Nhom2Context.AccountsUsers.FirstOrDefault(m => m.Id == idusser);
				DateTime now = DateTime.Now;

				// Kiểm tra ngày đặt lịch không được là quá khứ
				if (serviceBooking.BookingDate.HasValue && serviceBooking.BookingDate.Value < now.Date)
				{
					ViewData["Errdate"] = "Ngày đặt lịch không được là quá khứ.";
					return View();
				}

				// Kiểm tra thời gian đặt lịch không được là quá khứ
				//if (serviceBooking.BookingDate.HasValue && serviceBooking.BookingDate.Value.TimeOfDay < now.TimeOfDay)
				//{
				//	ViewData["Errdate"] = "Thời gian đặt lịch không được là quá khứ.";
				//	return View();
				//}

				// Kiểm tra ngày kết thúc đặt lịch không được là quá khứ
				if (serviceBooking.EndBookingDate.HasValue && serviceBooking.EndBookingDate.Value < now.Date)
				{
					ViewData["Errdate"] = "Ngày kết thúc đặt lịch không được là quá khứ.";
					return View();
				}

				//// Kiểm tra thời gian kết thúc đặt lịch không được là quá khứ
				//if (serviceBooking.EndBookingDate.HasValue && serviceBooking.EndBookingDate.Value.TimeOfDay < now.TimeOfDay)
				//{
				//	ViewData["Errdate"] = "Thời gian kết thúc đặt lịch không được là quá khứ.";
				//	return View();
				//}

				try
				{
					Order order = new Order
					{
						AccountId = idusser,
						PaymentDate = DateTime.Now,
						Deletee = false,
						Paid = true,
					};
					_project3Nhom2Context.Orders.Add(order);
					_project3Nhom2Context.SaveChanges();

					serviceBooking.Price = decimal.Parse(price);
					serviceBooking.ServiceId = idServicess;
					serviceBooking.Confirm = false;
					serviceBooking.OrderId = order.Id;
					serviceBooking.BookingStatus = true;
					serviceBooking.NumberOfGuards = package;

					_project3Nhom2Context.ServiceBookings.Add(serviceBooking);
					_project3Nhom2Context.SaveChanges();

					numberofpeopel.Numberofprople -= serviceBooking.NumberOfGuards;
					_project3Nhom2Context.Servicesses.Update(numberofpeopel);
					_project3Nhom2Context.SaveChanges();

					TempData["SuccessMessage"] = "Bạn Cần Chờ Admin Duyệt";
				}
				catch (Exception)
				{
					throw;
				}
				//}
				//else
				//{
				//	ViewData["Err"] = "Không đủ số lượng số lượng người chỉ còn: " + numberofpeopel.Numberofprople;
				//}
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Datngay1(DateTime servicebooking, int? idServicess)
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				if (idusser != null)
				{
					DateTime now = DateTime.Now;

					// Kiểm tra ngày đặt lịch không được là quá khứ
					if (servicebooking < now)
					{
						ViewData["Errdate"] = "Ngày đặt lịch không được là quá khứ.";
					}
					else
					{
						try
						{
							Order order = new Order
							{
								AccountId = idusser,
								PaymentDate = DateTime.Now,
								Deletee = false,
								Paid = true,
							};
							_project3Nhom2Context.Orders.Add(order);
							_project3Nhom2Context.SaveChanges();

							ServiceBooking serviceBooking = new ServiceBooking
							{
								ServiceId = idServicess,
								OrderId = order.Id,
								BookingDate = servicebooking,
								EndBookingDate = null,
								BookingStatus = true,
								Price = null,
								NumberOfGuards = null,
								Confirm = true,
								Status = false
							};
							_project3Nhom2Context.ServiceBookings.Add(serviceBooking);
							_project3Nhom2Context.SaveChanges();
							ViewData["Success"] = "Đặt Lịch Thành Công!";
						}
						catch (Exception)
						{
							throw;
						}

					}
				}
				return RedirectToAction("index");
			};

			// Chuyển hướng tới action "Chitiet" với tham số idServicess
			return RedirectToAction("Dangnhap");
		}
		//css trang nhắn tin

		[HttpGet]
		public IActionResult Tuyennhansu()
		{
			ViewData["DepartmentId"] = new SelectList(_project3Nhom2Context.Departments, "DepartmentId", "Location");
			ViewData["RoleID"] = new SelectList(_project3Nhom2Context.Roles, "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Tuyennhansu([Bind("RecruitmentId,DepartmentId,EmployeeName,EmployeeAddress,Phone,Avata,EducationalQualification,Gender,Birthday")] Recruitment recruitment, IFormFile img)
		{
			if (ModelState.IsValid)
			{
				if (img != null && img.Length > 0)
				{
					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/Avata");
					string uniqueFileName = /*Guid.NewGuid().ToString() + "_" + */img.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await img.CopyToAsync(fileStream);
					}
					recruitment.Avata = /*"/img/" +*/ uniqueFileName;
				}
				recruitment.Active = false;
				_project3Nhom2Context.Add(recruitment);
				await _project3Nhom2Context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["DepartmentId"] = new SelectList(_project3Nhom2Context.Departments, "DepartmentId", "Location");
			ViewData["RoleID"] = new SelectList(_project3Nhom2Context.Roles, "ID", "Name");
			return View(recruitment);
		}

		[HttpGet]
		public IActionResult DangNhap()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> DangNhap(string phone, string pass)
		{
			pass = GetMd5Hash(pass);
			var user = _project3Nhom2Context.AccountsUsers.FirstOrDefault(h => h.Phone.Equals(phone) && h.PassWord.Equals(pass));
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim("name",user.FullName),
					new Claim("avatar", user.Avata),
					new Claim("idusser", user.Id.ToString()),
					new Claim("permission","user")
				};
				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
				ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
				// Đăng nhập người dùng
				await HttpContext.SignInAsync(claimsPrincipal);
				int UserId = (int)user.Id;
				HttpContext.Session.SetInt32("user_id", UserId);
				return RedirectToAction("Index");
			}
			else
			{
				TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu không đúng.";
				return View();
			}
		}

		[HttpGet]
		public IActionResult Dangky()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DangKy([Bind("Id,FullName,Phone,PassWord")] AccountsUser accountsUser)
		{
			if (ModelState.IsValid)
			{
				if (accountsUser.Avata == null)
				{
					accountsUser.Avata = "user.png";
				}

				// Mã hoá mật khẩu bằng MD5
				accountsUser.PassWord = GetMd5Hash(accountsUser.PassWord);

				accountsUser.CreataDate = DateTime.Now;
				_project3Nhom2Context.AccountsUsers.Add(accountsUser);
				_project3Nhom2Context.SaveChanges();
				return RedirectToAction(nameof(DangNhap));
			}
			return View(accountsUser);
		}


		// Hàm mã hoá MD5 giống như đã viết ở trên
		public static string GetMd5Hash(string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder builder = new StringBuilder();

				for (int i = 0; i < data.Length; i++)
				{
					builder.Append(data[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public IActionResult Hoso()
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				var user = _project3Nhom2Context.AccountsUsers.Where(m => m.Id == idusser).FirstOrDefault();
				return View(user);
			}
			else
			{
				return RedirectToAction("DangNhap", "Home", new { area = "" });
			}

		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Hoso(int id, [Bind("Id,FullName,Phone,PassWord,Birthday,Address,Email,Avata,Gender,CreataDate")] AccountsUser accountsUser)
		{
			if (id != accountsUser.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_project3Nhom2Context.Update(accountsUser);
					await _project3Nhom2Context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{

				}
				return RedirectToAction(nameof(Index));
			}
			return View(accountsUser);
		}

		public IActionResult DichVu()
		{
			var dichvu = _project3Nhom2Context.Businesses.Include(m => m.Servicesses).ToList();
			return View(dichvu);
		}

		[HttpPost]
		public IActionResult BinhLuan([Bind("TestimonialId")] Testimonial testimonial, int ServiceID, string bl)
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				testimonial.AccountsUserId = idusser;
				testimonial.ServiceId = ServiceID;
				testimonial.TestimonialText = bl;
				_project3Nhom2Context.Testimonials.Add(testimonial);
				_project3Nhom2Context.SaveChanges();
				return RedirectToAction("Chitiet", new { id = ServiceID });
			}
			return RedirectToAction("DangNhap");
		}

		[HttpPost]
		public IActionResult Chatnew(string Content)
		{
			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "idusser");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int idusser))
			{
				try
				{
					Chat chat = new Chat
					{
						Contents = Content,
						UserId = idusser,
						Datetime = DateTime.Now,
					};
					_project3Nhom2Context.Add(chat);
					_project3Nhom2Context.SaveChanges();
				}
				catch (Exception ex)
				{

				}
				return RedirectToAction("Index");
			}
			return RedirectToAction("DangNhap");

		}

		public IActionResult News()
		{
			var news = _project3Nhom2Context.News.Include(m => m.Employess).ToList();
			return View(news);
		}

		[HttpGet]
		public IActionResult package()
		{
			List<Servicess> Servicess = _project3Nhom2Context.Servicesses.Where(m => m.BusinesId == 1).ToList();
			return View(Servicess);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}