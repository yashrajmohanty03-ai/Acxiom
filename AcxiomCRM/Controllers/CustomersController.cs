
using AcxiomCRM.Data;
using AcxiomCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcxiomCRM.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    (c.Company != null && c.Company.Contains(search)));
            }

            ViewData["Search"] = search;

            return View(await customers
                .OrderBy(c => c.Name)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            bool duplicateEmail = await _context.Customers
                .AnyAsync(c => c.Email == customer.Email);

            if (duplicateEmail)
            {
                ModelState.AddModelError(
                    nameof(customer.Email),
                    "A customer with this email already exists.");

                return View(customer);
            }

            customer.CreatedAt = DateTime.UtcNow;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, Customer customer)
        {
            if (id != customer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            bool duplicateEmail = await _context.Customers
                .AnyAsync(c => c.Email == customer.Email &&
                               c.Id != customer.Id);

            if (duplicateEmail)
            {
                ModelState.AddModelError(
                    nameof(customer.Email),
                    "This email is already used by another customer.");

                return View(customer);
            }

            var existing = await _context.Customers.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.Name = customer.Name;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.Company = customer.Company;
            existing.Address = customer.Address;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Customer updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Customer deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
