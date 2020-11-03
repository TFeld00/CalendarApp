using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;
using System;
using System.Linq;

namespace DotNetCoreSqlDb.Controllers
{
    public class TasksController : Controller
    {
        private readonly MyDatabaseContext _context;

        public TasksController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Calendar/AddTask
        public async Task<IActionResult> Create(int resourceId, DateTime date)
        {
            var resource = await _context.Resources.FindAsync(resourceId);

            var task = new Models.Task
            {
                Resource = resource,
                ResourceId = resourceId,
                Date = date
            };
            return View(task);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Date,ResourceId,TaskColor")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                var resource = await _context.Resources.FindAsync(task.ResourceId);
                return RedirectToAction(nameof(Index), "Calendar", new { team = resource.Team });
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.Include(t => t.Resource).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,ResourceId,TaskColor")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tasks.Any(e => e.Id == task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var resource = await _context.Resources.FindAsync(task.ResourceId);
                return RedirectToAction(nameof(Index), "Calendar", new { team = resource.Team });
            }
            return View(task);
        }


        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.Include(t => t.Resource).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.Include(t => t.Resource).FirstOrDefaultAsync(t => t.Id == id);
            var team = task.Resource.Team;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Calendar", new { team });
        }

    }
}
