using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;

namespace YhcdWebsite.Controllers
{
    public class AdminBugsController : Controller
    {
        private readonly CdyhcdDBContext _context;

        public AdminBugsController(CdyhcdDBContext context)
        {
            _context = context;
        }

        // GET: AdminBugs
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminBug.ToListAsync());
        }

        // GET: AdminBugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminBug = await _context.AdminBug
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adminBug == null)
            {
                return NotFound();
            }

            return View(adminBug);
        }

        // GET: AdminBugs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminBugs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserIp,BugInfo,BugMessage,IsShow,IsSolve,AddTime,EditTime")] AdminBug adminBug)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminBug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminBug);
        }

        // GET: AdminBugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminBug = await _context.AdminBug.SingleOrDefaultAsync(m => m.Id == id);
            if (adminBug == null)
            {
                return NotFound();
            }
            return View(adminBug);
        }

        // POST: AdminBugs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserIp,BugInfo,BugMessage,IsShow,IsSolve,AddTime,EditTime")] AdminBug adminBug)
        {
            if (id != adminBug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminBug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminBugExists(adminBug.Id))
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
            return View(adminBug);
        }

        // GET: AdminBugs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminBug = await _context.AdminBug
                .SingleOrDefaultAsync(m => m.Id == id);
            if (adminBug == null)
            {
                return NotFound();
            }

            return View(adminBug);
        }

        // POST: AdminBugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminBug = await _context.AdminBug.SingleOrDefaultAsync(m => m.Id == id);
            _context.AdminBug.Remove(adminBug);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminBugExists(int id)
        {
            return _context.AdminBug.Any(e => e.Id == id);
        }
    }
}
