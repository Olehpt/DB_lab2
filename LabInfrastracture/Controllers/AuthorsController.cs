using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabDomain.Model;
using LabInfrastructure;

namespace LabInfrastructure.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly DbLab2Context _context;

        public AuthorsController(DbLab2Context context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var dbLab2Context = _context.Authors.Include(a => a.OrganizationNavigation);
            return View(await dbLab2Context.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.OrganizationNavigation)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            ViewData["Organization"] = new SelectList(_context.Organizations, "OrganizationId", "Name");
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Password,Info,SignUpDate,Organization")] Author author)
        {
            var existingUser = await _context.Authors
                 .Where(u => EF.Functions.Like(u.Email, author.Email))
                 .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(author);
            }
            if (_context.Authors.Any()) author.AuthorId = _context.Authors.Max(a => a.AuthorId) + 1;
            else author.AuthorId = 1;
            if (author.SignUpDate > DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError(nameof(author.SignUpDate), "Not actual date.");
                return View(author);
            }
            var mindate = new DateOnly(2000, 1, 1);
            if (author.SignUpDate < mindate)
            {
                ModelState.AddModelError(nameof(author.SignUpDate), "Not actual date.");
                return View(author);
            }
            //
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["Organization"] = new SelectList(_context.Organizations, "OrganizationId", "Name", author.Organization);
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name,Email,Password,Info,SignUpDate,Organization")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            ViewData["Organization"] = new SelectList(_context.Organizations, "OrganizationId", "Name", author.Organization);
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.OrganizationNavigation)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}
