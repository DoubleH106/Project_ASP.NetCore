using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenQuangHuong.Models;

namespace NguyenQuangHuong.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class RecruitmentsController : Controller
	{
		private readonly Project3Nhom2Context _context;

		public RecruitmentsController(Project3Nhom2Context context)
		{
			_context = context;
		}

		// GET: Admin/Recruitments
		public async Task<IActionResult> Index()
		{
			var project3Nhom2Context = _context.Recruitments.Include(r => r.Department).Where(m => m.Active == false);
			return View(await project3Nhom2Context.ToListAsync());
		}

		public async Task<IActionResult> Admit(int? id)
		{
			var Reid = _context.Recruitments.Where(m => m.RecruitmentId == id).FirstOrDefault();
			if (Reid != null)
			{
				Employee employee = new Employee
				{
					FullName = Reid.EmployeeName,
					Address = Reid.EmployeeAddress,
					Phone = Reid.Phone,
					EducationalQualification = Reid.EducationalQualification,
					Avata = Reid.Avata,
					DepartmentId = Reid.DepartmentId,
					CreateDate = DateTime.Now,
					LastLogin = DateTime.Now,
					Birthday = Reid.Birthday,
					Email = Reid.Email,
					//Gender = Reid.Gender,
				};
				_context.Add(employee);
				_context.SaveChanges();
				Reid.Active = true;
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index", "Employees");
		}

		// GET: Admin/Recruitments/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Recruitments == null)
			{
				return NotFound();
			}

			var recruitment = await _context.Recruitments
				.Include(r => r.Department)
				.FirstOrDefaultAsync(m => m.RecruitmentId == id);
			if (recruitment == null)
			{
				return NotFound();
			}

			return View(recruitment);
		}

		// GET: Admin/Recruitments/Create
		public IActionResult Create()
		{
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
			return View();
		}

		// POST: Admin/Recruitments/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("RecruitmentId,DepartmentId,EmployeeName,EmployeeAddress,Phone,Avata,EducationalQualification,Active,Birthday,Email,Gender")] Recruitment recruitment)
		{
			if (ModelState.IsValid)
			{
				_context.Add(recruitment);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", recruitment.DepartmentId);
			return View(recruitment);
		}

		// GET: Admin/Recruitments/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Recruitments == null)
			{
				return NotFound();
			}

			var recruitment = await _context.Recruitments.FindAsync(id);
			if (recruitment == null)
			{
				return NotFound();
			}
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", recruitment.DepartmentId);
			return View(recruitment);
		}

		// POST: Admin/Recruitments/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("RecruitmentId,DepartmentId,EmployeeName,EmployeeAddress,Phone,Avata,EducationalQualification,Active,Birthday,Email,Gender")] Recruitment recruitment)
		{
			if (id != recruitment.RecruitmentId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(recruitment);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RecruitmentExists(recruitment.RecruitmentId))
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
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", recruitment.DepartmentId);
			return View(recruitment);
		}

		// GET: Admin/Recruitments/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Recruitments == null)
			{
				return NotFound();
			}

			var recruitment = await _context.Recruitments
				.Include(r => r.Department)
				.FirstOrDefaultAsync(m => m.RecruitmentId == id);
			if (recruitment == null)
			{
				return NotFound();
			}

			return View(recruitment);
		}

		// POST: Admin/Recruitments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Recruitments == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.Recruitments'  is null.");
			}
			var recruitment = await _context.Recruitments.FindAsync(id);
			if (recruitment != null)
			{
				_context.Recruitments.Remove(recruitment);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool RecruitmentExists(int id)
		{
			return (_context.Recruitments?.Any(e => e.RecruitmentId == id)).GetValueOrDefault();
		}
	}
}
