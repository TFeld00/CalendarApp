using DotNetCoreSqlDb.Helpers;
using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreSqlDb.Controllers
{
    //[Route("{team}/[controller]/[action]")]
    public class CalendarController : Controller
    {
        private readonly MyDatabaseContext _context;

        public CalendarController(MyDatabaseContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Calendar
        //[Route("~/{team}")]
        public async Task<IActionResult> Index(string team, string all, int? width)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
            if (width.HasValue) { ViewBag.Width = width; }
            var W = width ?? 2;

            IQueryable<Resource> resourceQuery = _context.Resources;

            var showAll = !string.IsNullOrEmpty(all);

            bool noTeam = string.IsNullOrWhiteSpace(team);
            if (!noTeam || !showAll)
            {
                resourceQuery = resourceQuery.Where(r => r.TeamName.Equals(team));
            }

            resourceQuery = resourceQuery.Include(r => r.Tasks)
                .OrderBy(r => r.TeamName)
                .ThenBy(r => r.Name);

            var x =
               from r in resourceQuery
               join t in _context.Teams on r.TeamName equals t.Name into rt
               from t in rt.DefaultIfEmpty()
               select new { r, t };

            List<Resource> resources = new List<Resource>();
            foreach (var rt in await x.ToListAsync())
            {
                rt.r.Team = rt.t;
                resources.Add(rt.r);
            }

            var startDate = DateTime.Today;
            var startWeek = startDate.StartOfWeek(DayOfWeek.Monday).AddDays(6);
            var endDate = startWeek.AddDays(W * 7);

            var holidays = _context.Holidays.Where(h => startDate <= h.Date && h.Date <= endDate).Select(h => h.Date).ToList();

            var vm = new CalendarViewModel
            {
                Team = team,
                Resources = resources,
                StartDate = startDate,
                EndDate = endDate,
                Holidays = holidays,
                NoTeam = noTeam,
                ShowAll = showAll
            };
            return View(vm);
        }

        // GET: Calendar/Create
        public IActionResult Create(string team, int? width)
        {
            ViewBag.Team = team;
            ViewBag.Width = width;
            var resource = new Resource
            {
                TeamName = team
            };
            return View(resource);
        }

        // POST: Calendar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string team, [Bind("Id,Name,TeamName")] Resource resource, string all, int? width)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
            ViewBag.Width = width;
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { team, all, width });
            }
            return View(resource);
        }

        // GET: Calendar/Edit/5
        public async Task<IActionResult> Edit(int? id, string team, string all, int? width)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
            ViewBag.Width = width;
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        // POST: Calendar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string team, int id, [Bind("Id,Name,TeamName")] Resource resource, string all, int? width)
        {

            ViewBag.Team = team;
            ViewBag.All = all;
            ViewBag.Width = width;
            if (id != resource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Resources.Any(e => e.Id == resource.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { team, all, width });
            }
            return View(resource);
        }


        // GET: Calendar/Delete/5
        public async Task<IActionResult> Delete(int? id, string team, string all, int? width)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
            ViewBag.Width = width;
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Resources.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Calendar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string team, int id, string all, int? width)
        {
            var resource = await _context.Resources.FindAsync(id);
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { team, all, width });
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
