using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenQuangHuong.Models;

namespace NguyenQuangHuong.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class EmployeesController : Controller
	{
		private readonly Project3Nhom2Context _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public EmployeesController(Project3Nhom2Context context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Admin/Employees
		public async Task<IActionResult> Index()
		{
			var project3Nhom2Context = _context.Employees.Include(e => e.Branch).Include(e => e.Department).Include(e => e.Role).Where(m => m.Active == true);
			return View(await project3Nhom2Context.ToListAsync());
		}

		// GET: Admin/Employees/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Employees == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Branch)
				.Include(e => e.Department)
				.Include(e => e.Role)
				.FirstOrDefaultAsync(m => m.UserId == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Admin/Employees/Create
		public IActionResult Create()
		{
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
			return View();
		}

		// POST: Admin/Employees/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserId,FullName,Phone,Email,Password,Gender,Birthday,Avata,Address,LastLogin,CreateDate,RoleId,EducationalQualification,DepartmentId,Achievements,Active,BranchId")] Employee employee, IFormFile img)
		{
			if (ModelState.IsValid)
			{
				if (img != null && img.Length > 0)
				{
					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
					string uniqueFileName = /*Guid.NewGuid().ToString() + "_" + */img.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await img.CopyToAsync(fileStream);
					}
					employee.Avata = /*"/img/" +*/ uniqueFileName;
				}
				// Mã hoá mật khẩu bằng MD5
				employee.Password = GetMd5Hash(employee.Password);
				_context.Add(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", employee.BranchId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employee.DepartmentId);
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", employee.RoleId);
			return View(employee);
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

		// GET: Admin/Employees/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Employees == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", employee.BranchId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employee.DepartmentId);
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", employee.RoleId);
			return View(employee);
		}

		// POST: Admin/Employees/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("UserId,FullName,Phone,Email,Password,Gender,Birthday,Avata,Address,LastLogin,CreateDate,RoleId,EducationalQualification,DepartmentId,Achievements,Active,BranchId")] Employee employee)
		{
			if (id != employee.UserId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(employee);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmployeeExists(employee.UserId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", employee.BranchId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employee.DepartmentId);
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", employee.RoleId);
			return View(employee);
		}

		// GET: Admin/Employees/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (_context.Employees == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.Employees'  is null.");
			}
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				employee.Active = false;
				_context.Update(employee);
			}
			HttpContext.Session.SetString("DeleteMessage", "Bạn đã xoá nhân viên: " + employee.FullName);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult IndexProtect()
		{
			var Protect = _context.Employees.Include(e => e.Branch).Include(e => e.Department).Include(e => e.Role).Where(m => m.Active == true && m.DepartmentId == 1).ToList();
			return View(Protect);
		}
		public IActionResult IndexATM()
		{
			var Protect = _context.Employees.Include(e => e.Branch).Include(e => e.Department).Include(e => e.Role).Where(m => m.Active == true && m.DepartmentId == 4).ToList();
			return View(Protect);
		}

		[HttpGet]
		public IActionResult CreateProtect()
		{
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateProtect([Bind("UserId,FullName,Phone,Email,Gender,Birthday,Avata,Address,EducationalQualification,Achievements,BranchId")] Employee employee, IFormFile img)
		{
			if (ModelState.IsValid)
			{
				if (img != null && img.Length > 0)
				{
					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
					string uniqueFileName = /*Guid.NewGuid().ToString() + "_" + */img.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await img.CopyToAsync(fileStream);
					}
					employee.Avata = /*"/img/" +*/ uniqueFileName;
				}
				employee.DepartmentId = 1;
				employee.Active = true;
				_context.Add(employee);
				employee.LastLogin = DateTime.Now;
				employee.CreateDate = DateTime.Now;
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", employee.BranchId);
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employee.DepartmentId);
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", employee.RoleId);
			return View(employee);
		}

		[HttpGet]
		public IActionResult listEmployeesdelete()
		{
			var Protect = _context.Employees.Include(e => e.Branch).Include(e => e.Department).Include(e => e.Role).Where(m => m.Active == false).ToList();
			return View(Protect);
		}

		public async Task<IActionResult> EditProtect(int? id)
		{
			if (_context.Employees == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.Employees'  is null.");
			}
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				employee.Active = true;
				_context.Update(employee);
			}
			HttpContext.Session.SetString("DeleteMessage", "Bạn đã Thêm nhân viên: " + employee.FullName);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool EmployeeExists(int id)
		{
			return (_context.Employees?.Any(e => e.UserId == id)).GetValueOrDefault();
		}
	}
}
