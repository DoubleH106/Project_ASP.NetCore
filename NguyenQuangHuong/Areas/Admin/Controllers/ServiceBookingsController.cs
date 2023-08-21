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
    public class ServiceBookingsController : Controller
    {
        private readonly Project3Nhom2Context _context;

        public ServiceBookingsController(Project3Nhom2Context context)
        {
            _context = context;
        }

        // GET: Admin/ServiceBookings
        public async Task<IActionResult> Index()
        {
            var project3Nhom2Context = _context.ServiceBookings.Include(s => s.Order).Include(s => s.Service);
            return View(await project3Nhom2Context.ToListAsync());
        }

        // GET: Admin/ServiceBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceBookings == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Order)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // GET: Admin/ServiceBookings/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["ServiceId"] = new SelectList(_context.Servicesses, "ServiceId", "ServiceId");
            return View();
        }

        // POST: Admin/ServiceBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,ServiceId,BookingDate,EndBookingDate,BookingStatus,Price,NumberOfGuards,OrderId")] ServiceBooking serviceBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", serviceBooking.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.Servicesses, "ServiceId", "ServiceId", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // GET: Admin/ServiceBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceBookings == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings.FindAsync(id);
            if (serviceBooking == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", serviceBooking.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.Servicesses, "ServiceId", "ServiceId", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // POST: Admin/ServiceBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,ServiceId,BookingDate,EndBookingDate,BookingStatus,Price,NumberOfGuards,OrderId")] ServiceBooking serviceBooking)
        {
            if (id != serviceBooking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceBookingExists(serviceBooking.BookingId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", serviceBooking.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.Servicesses, "ServiceId", "ServiceId", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // GET: Admin/ServiceBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceBookings == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Order)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // POST: Admin/ServiceBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceBookings == null)
            {
                return Problem("Entity set 'Project3Nhom2Context.ServiceBookings'  is null.");
            }
            var serviceBooking = await _context.ServiceBookings.FindAsync(id);
            if (serviceBooking != null)
            {
                _context.ServiceBookings.Remove(serviceBooking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceBookingExists(int id)
        {
          return (_context.ServiceBookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
