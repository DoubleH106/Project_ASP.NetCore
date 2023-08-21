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
	public class NewsController : Controller
	{
		private readonly Project3Nhom2Context _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public NewsController(Project3Nhom2Context context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Admin/News
		public async Task<IActionResult> Index()
		{
			var project3Nhom2Context = _context.News.Include(n => n.Employess);
			return View(await project3Nhom2Context.ToListAsync());
		}

		// GET: Admin/News/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.News == null)
			{
				return NotFound();
			}

			var news = await _context.News
				.Include(n => n.Employess)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (news == null)
			{
				return NotFound();
			}

			return View(news);
		}

		// GET: Admin/News/Create
		public IActionResult Create()
		{
			ViewData["EmployessId"] = new SelectList(_context.Employees, "UserId", "FullName");
			return View();
		}

		// POST: Admin/News/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,EmployessId,Title,Content,ImageUrl,PublishedDate,Status")] News news, IFormFile img)
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
					news.ImageUrl = /*"/img/" +*/ uniqueFileName;
				}

				_context.Add(news);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["EmployessId"] = new SelectList(_context.Employees, "UserId", "UserId", news.EmployessId);
			return View(news);
		}

		// GET: Admin/News/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.News == null)
			{
				return NotFound();
			}

			var news = await _context.News.FindAsync(id);
			if (news == null)
			{
				return NotFound();
			}
			ViewData["EmployessId"] = new SelectList(_context.Employees, "UserId", "UserId", news.EmployessId);
			return View(news);
		}

		// POST: Admin/News/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,EmployessId,Title,Content,ImageUrl,PublishedDate,Status")] News news)
		{
			if (id != news.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(news);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!NewsExists(news.Id))
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
			ViewData["EmployessId"] = new SelectList(_context.Employees, "UserId", "UserId", news.EmployessId);
			return View(news);
		}

		// GET: Admin/News/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.News == null)
			{
				return NotFound();
			}

			var news = await _context.News
				.Include(n => n.Employess)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (news == null)
			{
				return NotFound();
			}

			return View(news);
		}

		// POST: Admin/News/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.News == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.News'  is null.");
			}
			var news = await _context.News.FindAsync(id);
			if (news != null)
			{
				_context.News.Remove(news);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool NewsExists(int id)
		{
			return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
