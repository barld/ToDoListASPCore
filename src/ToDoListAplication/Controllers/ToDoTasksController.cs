using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoListAplication.Data;
using ToDoListAplication.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ToDoListAplication.Controllers
{
    [Authorize]
    public class ToDoTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ToDoTasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDoTaks
        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);
            var applicationDbContext = _context.ToDoTasks.Include(t => t.ToDoList);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ToDoTaks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTaks = await _context.ToDoTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoTaks == null)
            {
                return NotFound();
            }

            return View(toDoTaks);
        }

        // GET: ToDoTaks/Create
        public IActionResult Create()
        {
            ViewData["ToDoListId"] = new SelectList(_context.ToDoList, "Id", "ToDoList");
            return View();
        }

        // POST: ToDoTaks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Done,Task,ToDoListId")] ToDoTasks toDoTaks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoTaks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ToDoListId"] = new SelectList(_context.ToDoList, "Id", "ToDoList", toDoTaks.ToDoListId);
            return View(toDoTaks);
        }

        // GET: ToDoTaks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTaks = await _context.ToDoTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoTaks == null)
            {
                return NotFound();
            }
            ViewData["ToDoListId"] = new SelectList(_context.ToDoList, "Id", "ToDoList", toDoTaks.ToDoListId);
            return View(toDoTaks);
        }

        // POST: ToDoTaks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Done,Task,ToDoListId")] ToDoTasks toDoTaks)
        {
            if (id != toDoTaks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoTaks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoTaksExists(toDoTaks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ToDoListId"] = new SelectList(_context.ToDoList, "Id", "ToDoList", toDoTaks.ToDoListId);
            return View(toDoTaks);
        }

        // GET: ToDoTaks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTaks = await _context.ToDoTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoTaks == null)
            {
                return NotFound();
            }

            return View(toDoTaks);
        }

        // POST: ToDoTaks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoTaks = await _context.ToDoTasks.SingleOrDefaultAsync(m => m.Id == id);
            _context.ToDoTasks.Remove(toDoTaks);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ToDoTaksExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}
