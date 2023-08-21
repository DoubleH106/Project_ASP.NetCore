using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenQuangHuong.Models;
using NguyenQuangHuong.unitity;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NguyenQuangHuong.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		private readonly Project3Nhom2Context _context;

		public HomeController(Project3Nhom2Context nhom2Context)
		{
			_context = nhom2Context;
		}
		[Authenticated]
		public IActionResult Index()
		{
			var tong = _context.ServiceBookings
					.Where(m => m.Confirm == true)
					.Sum(m => m.Price);

			var permissionClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "permission");
			if (permissionClaim != null && int.TryParse(permissionClaim.Value, out int permission))
			{
				ViewData["sapxep"] = _context.Servicesses.OrderByDescending(item => item.Views).Where(m => m.Views > 0).Take(3);
				ViewData["tong"] = tong;
			}
			else
			{
				ViewData["Err"] = "Bạn không được phép truy cập!!!";
			}
			return View();
		}

		[HttpGet]
		public IActionResult DuyetAdmin()
		{
			var order = _context.ServiceBookings.Include(m => m.Service).Include(m => m.Order).ThenInclude(m => m.Account).Where(m => m.Status == false && m.Confirm == false);
			return View(order);
		}

		public async Task<IActionResult> EditDuyet(int? id)
		{
			var duyet = _context.ServiceBookings.Where(m => m.BookingId == id).FirstOrDefault();
			Servicess servicess = _context.Servicesses.Where(m => m.ServiceId == duyet.ServiceId).FirstOrDefault();
			if (duyet != null)
			{
				servicess.Views++;
				_context.Servicesses.Update(servicess);
				await _context.SaveChangesAsync();
				duyet.Confirm = true;
				_context.ServiceBookings.Update(duyet);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("DuyetAdmin");
		}

		public async Task<IActionResult> Huydichvu(int? id)
		{
			var duyet = _context.ServiceBookings.Include(m => m.Service).FirstOrDefault(m => m.BookingId == id);
			if (duyet != null)
			{
				duyet.Status = true;
				_context.ServiceBookings.Update(duyet);
				await _context.SaveChangesAsync();
				var chitiet = _context.Servicesses.FirstOrDefault(m => m.ServiceId == duyet.ServiceId);
				if (chitiet != null)
				{
					chitiet.Numberofprople += duyet.NumberOfGuards;
					_context.Servicesses.Update(chitiet);
					await _context.SaveChangesAsync();
				}
				//HttpContext.Session.SetString("Huyservice", "Dịch Vụ Của Bạn Bị Từ Chối!!!");
				ViewData["Err"] = "Dịch vụ của bạn bị từ chối";
			}
			return RedirectToAction("DuyetAdmin");
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.ServiceBookings == null)
			{
				return NotFound();
			}

			var accountsUser = await _context.ServiceBookings.Include(m => m.Service).Include(m => m.Order).ThenInclude(m => m.Account)
				.FirstOrDefaultAsync(m => m.BookingId == id);
			if (accountsUser == null)
			{
				return NotFound();
			}
			return View(accountsUser);
		}

		[HttpGet]
		public IActionResult DangnhapAdmin()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DangnhapAdmin(string phone, string pass)
		{
			var check = _context.Employees.FirstOrDefault(m => m.Phone.Equals(phone) && m.Password.Equals(pass));
			if (check != null)
			{
				if (check.RoleId != null)
				{
					var claims = new List<Claim>
				{
					new Claim("nameadmin", check.FullName),
					new Claim("avataradmin", check.Avata),
					new Claim("permission", check.RoleId.ToString())
				};
					ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "loginAdmin");
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					// Đăng nhập người dùng
					await HttpContext.SignInAsync(claimsPrincipal);
					return RedirectToAction("Index", "Home", new { area = "Admin" });
				}
				else
				{
					return RedirectToAction("DangnhapAdmin", "Home", new { area = "Admin" });
				}
			}
			else
			{
				return RedirectToAction("DangnhapAdmin", "Home", new { area = "Admin" });
			}
		}
	}
}


