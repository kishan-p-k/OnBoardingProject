namespace BT.Models;

public class BugFilter
{
    public string? reference_id {  get; set; }
    public string? Keyword { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public string? CreatedBy { get; set; }
    public string? Assignee { get; set; }
    public string? Search { get; set; }
}