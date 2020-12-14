using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using System;
using DotNetCoreSqlDb.Helpers;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

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
        public async Task<IActionResult> Index(string team, string all, int width = 14)
        {
            ViewBag.All = all;
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

            var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var endDate = startDate.AddDays(width - 1);

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
        public IActionResult Create(string team)
        {
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
        public async Task<IActionResult> Create(string team, [Bind("Id,Name,TeamName")] Resource resource, string all)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { team, all });
            }
            return View(resource);
        }

        // GET: Calendar/Edit/5
        public async Task<IActionResult> Edit(int? id, string team, string all)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
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
        public async Task<IActionResult> Edit(string teamPage, int id, [Bind("Id,Name,TeamName")] Resource resource, string all)
        {
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
                return RedirectToAction(nameof(Index), new { team = teamPage, all });
            }
            return View(resource);
        }


        // GET: Calendar/Delete/5
        public async Task<IActionResult> Delete(int? id, string team, string all)
        {
            ViewBag.Team = team;
            ViewBag.All = all;
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
        public async Task<IActionResult> DeleteConfirmed(string team, int id, string all)
        {
            var resource = await _context.Resources.FindAsync(id);
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { team, all });
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
