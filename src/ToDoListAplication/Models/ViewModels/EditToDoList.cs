using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAplication.Models.ViewModels
{
    public class EditToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDoTasks> Tasks { get; set; }
    }
}
