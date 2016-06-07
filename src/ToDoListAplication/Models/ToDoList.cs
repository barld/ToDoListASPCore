using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAplication.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ToDoTasks> Tasks { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}
