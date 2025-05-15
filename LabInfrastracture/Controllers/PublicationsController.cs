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
    public class PublicationsController : Controller
    {
        private readonly DbLab2Context _context;

        public PublicationsController(DbLab2Context context)
        {
            _context = context;
        }

        // GET: Publications
        public async Task<IActionResult> Index()
        {
            var dbLab2Context = _context.Publications.Include(p => p.AuthorNavigation).Include(p => p.PublicationTypeNavigation).Include(p => p.SubjectNavigation);
            return View(await dbLab2Context.ToListAsync());
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .Include(p => p.AuthorNavigation)
                .Include(p => p.PublicationTypeNavigation)
                .Include(p => p.SubjectNavigation)
                .FirstOrDefaultAsync(m => m.PublicationId == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // GET: Publications/Create
        public IActionResult Create()
        {
            ViewData["Author"] = new SelectList(_context.Authors, "AuthorId", "Email");
            ViewData["PublicationType"] = new SelectList(_context.PublicationTypes, "PublicationTypeId", "Name");
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name");
            return View();
        }

        // POST: Publications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublicationId,Title,Content,PublicationDate,Views,Subject,PublicationType,Author")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Author"] = new SelectList(_context.Authors, "AuthorId", "Email", publication.Author);
            ViewData["PublicationType"] = new SelectList(_context.PublicationTypes, "PublicationTypeId", "Name", publication.PublicationType);
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", publication.Subject);
            return View(publication);
        }

        // GET: Publications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            ViewData["Author"] = new SelectList(_context.Authors, "AuthorId", "Email", publication.Author);
            ViewData["PublicationType"] = new SelectList(_context.PublicationTypes, "PublicationTypeId", "Name", publication.PublicationType);
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", publication.Subject);
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,Title,Content,PublicationDate,Views,Subject,PublicationType,Author")] Publication publication)
        {
            if (id != publication.PublicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationExists(publication.PublicationId))
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
            ViewData["Author"] = new SelectList(_context.Authors, "AuthorId", "Email", publication.Author);
            ViewData["PublicationType"] = new SelectList(_context.PublicationTypes, "PublicationTypeId", "Name", publication.PublicationType);
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", publication.Subject);
            return View(publication);
        }

        // GET: Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .Include(p => p.AuthorNavigation)
                .Include(p => p.PublicationTypeNavigation)
                .Include(p => p.SubjectNavigation)
                .FirstOrDefaultAsync(m => m.PublicationId == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }
}
