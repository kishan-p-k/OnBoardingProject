using System.ComponentModel.DataAnnotations;

namespace BT.Models;

public class Users  
{
    public string Reference_id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
    
    [Required] public string Mail { get; set; } = string.Empty;
    
    [Required] public string Password { get; set; } = string.Empty;
    
    public string role { get; set; } = string.Empty;
}
