using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAplication.Models.ViewModels
{
    public class CreatToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Tasks { get; set; }
    }
}
