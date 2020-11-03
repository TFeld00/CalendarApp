using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using System;
using DotNetCoreSqlDb.Helpers;
using System.Linq;
using System.Diagnostics;

namespace DotNetCoreSqlDb.Controllers
{
    [Route("{team}/[controller]/[action]")]
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
        [Route("~/{team}")]
        public async Task<IActionResult> Index(string team, int width = 14)
        {
            var resources = await _context.Resources
                .Where(r => r.Team.Equals(team))
                .Include(r => r.Tasks)
                .OrderBy(r => r.Name)
                .ToListAsync();
            var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

            var vm = new CalendarViewModel
            {
                Team = team,
                Resources = resources,
                StartDate = startDate,
                EndDate = startDate.AddDays(width - 1)
            };
            return View(vm);
        }

        // GET: Calendar/Create
        public IActionResult Create(string team)
        {
            var resource = new Resource
            {
                Team = team
            };
            return View(resource);
        }

        // POST: Calendar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string team, [Bind("Id,Name,Team")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { team });
            }
            return View(resource);
        }

        // GET: Calendar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(string team, int id, [Bind("Id,Name,Team")] Resource resource)
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
                return RedirectToAction(nameof(Index), new { team });
            }
            return View(resource);
        }


        // GET: Calendar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(string team, int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { team });
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
