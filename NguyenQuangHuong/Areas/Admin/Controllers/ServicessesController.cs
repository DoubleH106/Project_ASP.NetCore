using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenQuangHuong.Models;

namespace NguyenQuangHuong.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ServicessesController : Controller
	{
		private readonly Project3Nhom2Context _context;
		private readonly IWebHostEnvironment _environment;
		public ServicessesController(Project3Nhom2Context context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;

			_environment = webHostEnvironment;
		}

		// GET: Admin/Servicesses
		public async Task<IActionResult> Index()
		{
			var project3Nhom2Context = _context.Servicesses.Include(s => s.Busines);
			return View(await project3Nhom2Context.ToListAsync());
		}

		public async Task<IActionResult> Baove()
		{
			var baove = _context.Servicesses.Include(s => s.Busines).Where(m => m.BusinesId == 1).ToList();
			return View(baove);
		}

		public async Task<IActionResult> dichvutien()
		{
			var tien = _context.Servicesses.Include(s => s.Busines).Where(m => m.BusinesId == 3).ToList();
			return View(tien);
		}

		public async Task<IActionResult> cammera()
		{
			var cammera = _context.Servicesses.Include(s => s.Busines).Where(m => m.BusinesId == 4).ToList();
			return View(cammera);
		}

		// GET: Admin/Servicesses/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Servicesses == null)
			{
				return NotFound();
			}

			var servicess = await _context.Servicesses
				.Include(s => s.Busines)
				.FirstOrDefaultAsync(m => m.ServiceId == id);
			if (servicess == null)
			{
				return NotFound();
			}

			return View(servicess);
		}

		// GET: Admin/Servicesses/Create
		public IActionResult Create()
		{
			ViewData["BusinesId"] = new SelectList(_context.Businesses, "Id", "BusinesName");
			ViewData["TestimonialId"] = new SelectList(_context.Testimonials, "TestimonialId", "TestimonialId");
			return View();
		}

		// POST: Admin/Servicesses/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Avata,ServicePrice,Numberofprople,Description,BusinesId,TestimonialId,Package")] Servicess servicess, IFormFile img)
		{
			if (ModelState.IsValid)
			{
				if (img != null && img.Length > 0)
				{
					string uploadsFolder = Path.Combine(_environment.WebRootPath, "img");
					string uniqueFileName = /*Guid.NewGuid().ToString() + "_" + */img.FileName;
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await img.CopyToAsync(fileStream);
					}
					servicess.Avata = /*"/img/" +*/ uniqueFileName;
					servicess.Views = 0;
				}
				_context.Add(servicess);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["BusinesId"] = new SelectList(_context.Businesses, "Id", "Id", servicess.BusinesId);
			return View(servicess);
		}

		// GET: Admin/Servicesses/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Servicesses == null)
			{
				return NotFound();
			}

			var servicess = await _context.Servicesses.FindAsync(id);
			if (servicess == null)
			{
				return NotFound();
			}
			ViewData["BusinesId"] = new SelectList(_context.Businesses, "Id", "BusinesName", servicess.BusinesId);
			return View(servicess);
		}

		// POST: Admin/Servicesses/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Avata,ServicePrice,Numberofprople,Description,BusinesId,TestimonialId")] Servicess servicess)
		{
			if (id != servicess.ServiceId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(servicess);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ServicessExists(servicess.ServiceId))
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
			ViewData["BusinesId"] = new SelectList(_context.Businesses, "Id", "Id", servicess.BusinesId);
			return View(servicess);
		}

		// GET: Admin/Servicesses/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Servicesses == null)
			{
				return NotFound();
			}

			var servicess = await _context.Servicesses
				.Include(s => s.Busines)
				.FirstOrDefaultAsync(m => m.ServiceId == id);
			if (servicess == null)
			{
				return NotFound();
			}

			return View(servicess);
		}

		// POST: Admin/Servicesses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Servicesses == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.Servicesses'  is null.");
			}
			var servicess = await _context.Servicesses.FindAsync(id);
			if (servicess != null)
			{
				_context.Servicesses.Remove(servicess);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ServicessExists(int id)
		{
			return (_context.Servicesses?.Any(e => e.ServiceId == id)).GetValueOrDefault();
		}
	}
}
