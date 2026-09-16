namespace BT.Models;

public class Comment
{
    public string reference_id { get; set; } = string.Empty;
    public string comment { get; set; } = string.Empty;
    public string author { get; set; } = string.Empty;
    public DateTime date { get; set; } = DateTime.UtcNow;
}
