namespace ToDoBackend.Models
{
    public class ToDoItemDTO
    {
        public int id { get; set; }
        public string? title { get; set; }
        public bool isUrgent { get; set; }
        public bool isCompleted { get; set; }
        public DateTime createdAt { get; set; }
    }
}