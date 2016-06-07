using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListAplication.Data;
using ToDoListAplication.Models;

namespace ToDoListAplication.Controllers
{
    public interface IToDoTasksController
    {
        IActionResult Create();
        Task<IActionResult> Create([Bind(new[] { "Id,Done,Task,ToDoListId" })] ToDoTasks toDoTaks);
        Task<IActionResult> Delete(int? id);
        Task<IActionResult> DeleteConfirmed(int id);
        Task<IActionResult> Details(int? id);
        Task<IActionResult> Edit(int? id);
        Task<IActionResult> Edit(int id, [Bind(new[] { "Id,Done,Task,ToDoListId" })] ToDoTasks toDoTaks);
        Task<IActionResult> Index();
        void ToDoTaksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager);
    }
}