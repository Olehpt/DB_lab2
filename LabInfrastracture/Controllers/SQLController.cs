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
        public async Task<IActionResult> RequestSix()
        {
            var result = await _context.Authors
                .FromSqlInterpolated($"SELECT a.* FROM Author a WHERE NOT EXISTS (SELECT pt.PublicationTypeID FROM PublicationType pt WHERE NOT EXISTS (SELECT 1 FROM Publication p JOIN Comment c ON c.Publication = p.PublicationID WHERE p.PublicationType = pt.PublicationTypeID AND c.Author = a.AuthorID))")
                .ToListAsync();
            return View(result);
        }
        [HttpPost]
        public IActionResult RequestSixPost()
        {
            return RedirectToAction("RequestSix");
        }
        //7
        public async Task<IActionResult> RequestSeven()
        {
            var result = await _context.Subjects
                .FromSqlInterpolated($"SELECT s.*\r\nFROM Subject s\r\nWHERE NOT EXISTS (\r\n    SELECT p.PublicationID\r\n    FROM Publication p\r\n    WHERE p.Subject = s.SubjectID\r\n    AND NOT EXISTS (\r\n        SELECT 1\r\n        FROM Comment c\r\n        WHERE c.Publication = p.PublicationID\r\n    )\r\n);")
                .ToListAsync();
            return View(result);
        }
        [HttpPost]
        public IActionResult RequestSevenPost()
        {
            return RedirectToAction("RequestSeven");
        }
        //8
        public async Task<IActionResult> RequestEight(int? subjectid)
        {
            ViewData["Subject"] = new SelectList(_context.Subjects, "SubjectId", "Name", subjectid);
            if (subjectid != null)
            {
                var result = await _context.Comments
                .FromSqlInterpolated($"SELECT c.* FROM Comment c Join Publication p ON p.PublicationID = c.Publication Where p.Subject = {subjectid}")
                .ToListAsync();
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RequestEightPost(int subjectid)
        {
            return RedirectToAction("RequestEight", new { subjectid });
        }
    }
}
