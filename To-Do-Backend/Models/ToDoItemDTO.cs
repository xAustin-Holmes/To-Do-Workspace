namespace ToDoBackend.Models
{
    public class ToDoItemDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}