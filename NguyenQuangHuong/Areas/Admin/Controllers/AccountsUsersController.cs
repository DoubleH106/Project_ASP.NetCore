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
	public class AccountsUsersController : Controller
	{
		private readonly Project3Nhom2Context _context;
		private readonly IWebHostEnvironment _environment;

		public AccountsUsersController(Project3Nhom2Context context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_environment = webHostEnvironment;
		}

		// GET: Admin/AccountsUsers
		public async Task<IActionResult> Index()
		{
			return _context.AccountsUsers != null ?
						View(await _context.AccountsUsers.ToListAsync()) :
						Problem("Entity set 'Project3Nhom2Context.AccountsUsers'  is null.");
		}

		// GET: Admin/AccountsUsers/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.AccountsUsers == null)
			{
				return NotFound();
			}

			var accountsUser = await _context.AccountsUsers
				.FirstOrDefaultAsync(m => m.Id == id);
			if (accountsUser == null)
			{
				return NotFound();
			}

			return View(accountsUser);
		}

		// GET: Admin/AccountsUsers/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/AccountsUsers/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,FullName,Phone,PassWord,Birthday,Address,Email,Avata,Gender,CreataDate")] AccountsUser accountsUser, IFormFile img)
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
					accountsUser.Avata = /*"/img/" +*/ uniqueFileName;
				}
				//string salt = Utilities.GetRandomKey();
				//accountsUser.PassWord = (accountsUser.PassWord + pa)
				_context.Add(accountsUser);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(accountsUser);
		}

		// GET: Admin/AccountsUsers/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.AccountsUsers == null)
			{
				return NotFound();
			}

			var accountsUser = await _context.AccountsUsers.FindAsync(id);
			if (accountsUser == null)
			{
				return NotFound();
			}
			return View(accountsUser);
		}

		// POST: Admin/AccountsUsers/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Phone,PassWord,Birthday,Address,Email,Avata,Gender,CreataDate")] AccountsUser accountsUser)
		{
			if (id != accountsUser.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(accountsUser);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AccountsUserExists(accountsUser.Id))
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
			return View(accountsUser);
		}

		// GET: Admin/AccountsUsers/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.AccountsUsers == null)
			{
				return NotFound();
			}

			var accountsUser = await _context.AccountsUsers
				.FirstOrDefaultAsync(m => m.Id == id);
			if (accountsUser == null)
			{
				return NotFound();
			}

			return View(accountsUser);
		}

		// POST: Admin/AccountsUsers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.AccountsUsers == null)
			{
				return Problem("Entity set 'Project3Nhom2Context.AccountsUsers'  is null.");
			}
			var accountsUser = await _context.AccountsUsers.FindAsync(id);
			if (accountsUser != null)
			{
				_context.AccountsUsers.Remove(accountsUser);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult CreateProtect()
		{
			return View();
		}

		private bool AccountsUserExists(int id)
		{
			return (_context.AccountsUsers?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
