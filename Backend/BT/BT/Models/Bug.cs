namespace BT.Models
{
    public class Bug
    {
        public int BugId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int CreatedBy { get; set; } = 0;
        public int? Assignee { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
