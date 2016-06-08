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

using ToDoListAplication.Models.ViewModels;

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
            var applicationDbContext = _context.ToDoList
                .Include(t => t.Owner)
                .Where(t => t.ApplicationUserId == _userManager.GetUserId(User));
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: ToDoLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.ViewModels.CreatToDoList create)
        {
            if (ModelState.IsValid)
            {
                var toDoList = new ToDoList()
                {
                    ApplicationUserId = _userManager.GetUserId(User),
                    Name = create.Name,
                    Tasks = create.Tasks
                        .Where(s => !string.IsNullOrEmpty(s))
                        .Select(s => new ToDoTasks() { Done = false,Task = s}).ToList()
                };
                _context.Add(toDoList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(create);
        }

        // GET: ToDoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoList.Include(m => m.Tasks).SingleOrDefaultAsync(m => m.Id == id && m.ApplicationUserId == _userManager.GetUserId(User));
            if (toDoList == null)
            {
                return NotFound();
            }
            var viewModel = new EditToDoList() { Id = toDoList.Id, Name = toDoList.Name, Tasks = toDoList.Tasks.ToList() };
            viewModel.Tasks.Add(new ToDoTasks());
            viewModel.Tasks.Add(new ToDoTasks());
            return View(viewModel);
        }

        // POST: ToDoLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return NotFound();
            }          

            if (ModelState.IsValid)
            {
                var origenal = await _context.ToDoList.Include(tl => tl.Tasks)
                .FirstOrDefaultAsync(tl => tl.Id == id && tl.ApplicationUserId == _userManager.GetUserId(User));

                if(origenal != null)
                {
                    var newTaks = toDoList.Tasks.Where(nt => !string.IsNullOrWhiteSpace(nt.Task)).ToList();

                    origenal.Name = toDoList.Name;
                    
                    var tasks = origenal.Tasks.ToList()
                        .Where(t => newTaks.Exists(nt => nt.Id == t.Id)).ToList();
                    tasks.ForEach(t =>
                        {
                            var newtask = newTaks.Find(nt => nt.Id == nt.Id);
                            t.Done = newtask.Done;
                            t.Task = newtask.Task;
                            //_context.Entry(t).State = EntityState.Modified;
                            //_context.Update(t);
                        }
                    );
                    origenal.Tasks = tasks.ToList();


                    try
                    {
                        _context.Entry(origenal).State = EntityState.Modified;
                        _context.Update(origenal);
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

                
            }
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
