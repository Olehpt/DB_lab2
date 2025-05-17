using LabDomain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace LabInfrastructure.Controllers
{
    public class SQLController : Controller
    {
        private readonly DbLab2Context _context;
        public SQLController(DbLab2Context context)
        {
            _context = context;
        }
        //1
        [HttpGet]
        public async Task<IActionResult> RequestOne(int? item)
        {
            if (item != null)
            {
                ViewBag.Item = item; 
                var result = await _context.Publications
                .FromSqlInterpolated($"SELECT * FROM Publication WHERE Views >= {item}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestOnePost(int item)
        {
            return RedirectToAction("RequestOne", new { item });
        }
        //2
        [HttpGet]
        public async Task<IActionResult> RequestTwo(int? subjectid)
        {
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", subjectid);
            if (subjectid != null)
            {
                var result = await _context.Publications
                .FromSqlInterpolated($"SELECT * FROM Publication WHERE Subject = {subjectid}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestTwoPost(int subjectid)
        {
            return RedirectToAction("RequestTwo", new { subjectid });
        }
        //3
        [HttpGet]
        public async Task<IActionResult> RequestThree(int? typeid)
        {
            ViewData["Type"] = new SelectList(_context.PublicationTypes, "PublicationTypeId", "Name", typeid);
            if (typeid != null)
            {
                var result = await _context.Publications
                .FromSqlInterpolated($"SELECT * FROM Publication WHERE PublicationType = {typeid}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestThreePost(int typeid)
        {
            return RedirectToAction("RequestThree", new { typeid });
        }
        //4
        public async Task<IActionResult> RequestFour(int? item)
        {
            if (item != null)
            {
                ViewBag.Item = item;
                var result = await _context.Authors
                .FromSqlInterpolated($"SELECT a.* FROM Author a JOIN ( SELECT Author, COUNT(*) AS MentionCount FROM Publication GROUP BY Author HAVING COUNT(*) >= {item}) p ON a.AuthorID = p.Author")
                .ToListAsync();
                var authorids = result.Select(a => a.AuthorId).ToList();
                var P = await _context.Publications.Where(x => authorids.Contains(x.Author)).ToListAsync();
                var P0 = P.GroupBy(p => p.Author).ToDictionary(g => g.Key, g => g.Count());
                ViewBag.PublicationCounts = P0;
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestFourPost(int item)
        {
            return RedirectToAction("RequestFour", new {item});
        }
        //5
        [HttpGet]
        public async Task<IActionResult> RequestFive(string? content)
        {
            if (content != null)
            {
                ViewBag.Content = content;
                var pattern = $"%{content}%";
                var result = await _context.Authors
                .FromSqlInterpolated($"SELECT Distinct Author.* From Author inner join Comment on Author.AuthorID = Comment.Author Where Comment.Content Like {pattern} ")
                .ToListAsync();
                var comscontent = await _context.Comments.Where(x => x.Content.Contains(content)).Select(item => new {item.Content, item.Author}).ToListAsync();
                ViewBag.ComBag = comscontent;
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestFivePost(string content)
        {
            return RedirectToAction("RequestFive", new { content });
        }
        //6
        [HttpGet]
        public async Task<IActionResult> RequestSix(int? subjectid, int? amount)
        {
            if (amount == null) amount = 0;
            ViewBag.amount = amount;
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", subjectid);
            if (subjectid != null)
            {
                var result = await _context.Authors
                .FromSqlInterpolated($"SELECT a.AuthorID, a.Name, a.Email, a.Password, a.Info, a.SignUpDate, a.Organization FROM Author a Join Publication p ON p.Author = a.AuthorID Where p.Subject = {subjectid} Group By a.AuthorID, a.Name, a.Email, a.Password, a.Info, a.SignUpDate, a.Organization Having Count(p.PublicationID) >= {amount}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestSixPost(int subjectid, int amount)
        {
            return RedirectToAction("RequestSix", new { subjectid, amount });
        }
        //7
        public async Task<IActionResult> RequestSeven(int? subjectid, int? orgid)
        {
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", subjectid);
            ViewData["Organization"] = new SelectList(_context.Organizations, "OrganizationId", "Name", orgid);
            if (subjectid != null)
            {
                var result = await _context.Authors
                .FromSqlInterpolated($"SELECT a.* FROM Author a Join Publication p ON p.Author = a.AuthorID Where p.Subject = {subjectid} AND p.PublicationType = {orgid}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestSevenPost(int subjectid, int orgid)
        {
            return RedirectToAction("RequestSeven", new { subjectid, orgid });
        }
        //8
    }
}
