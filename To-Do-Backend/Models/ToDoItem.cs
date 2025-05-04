namespace ToDoBackend.Models
{
    public class ToDoItem
    {
        public int id { get; set; }
        public string? title { get; set; }
        public bool isUrgent { get; set; }
        public bool isCompleted { get; set; }
        public DateTime createdAt { get; set; }
        public string? secret { get; set; }
    }
}