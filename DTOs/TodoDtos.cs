namespace TodoAPI.DTOs
{
    public class TodoDto
    {
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class UpdateTodoDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
   