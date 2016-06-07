namespace ToDoListAplication.Models
{
    public class ToDoTasks
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool Done { get; set; }
        public int ToDoListId { get; set; }
        public virtual ToDoList ToDoList { get; set; }
    }
}