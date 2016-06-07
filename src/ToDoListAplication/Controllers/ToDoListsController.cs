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

namespace ToDoListAplication.Controllers
{
    [Authorize]
    public class ToDoListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ToDoListsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDoLists
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ToDoList.Include(t => t.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ToDoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        // GET: ToDoLists/Create
        public IActionResult Create()
        {
            var users = _context.Users.Select(u => new { Id = u.Id, Owner = u.UserName });
            ViewData["ApplicationUserId"] = new SelectList(users, "Id", "Owner");
            return View();
        }

        // POST: ToDoLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Name")] ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Owner", toDoList.ApplicationUserId);
            return View(toDoList);
        }

        // GET: ToDoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Owner", toDoList.ApplicationUserId);
            return View(toDoList);
        }

        // POST: ToDoLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Name")] ToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoListExists(toDoList.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Owner", toDoList.ApplicationUserId);
            return View(toDoList);
        }

        // GET: ToDoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        // POST: ToDoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoList = await _context.ToDoList.SingleOrDefaultAsync(m => m.Id == id);
            _context.ToDoList.Remove(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoList.Any(e => e.Id == id);
        }
    }
}
