namespace WebApplication1.Models
{
    public class Task
    {
        public string Kind = "tasks#task";
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public string Parent { get; set; } = "";
        public string? SelfLink { get; set;}
        public string Status { get; set; } = "needsAction";
        public bool Deleted { get; set; } = false;

        public static implicit operator List<object>(Task? v)
        {
            throw new NotImplementedException();
        }
    }
}
