using BT.Models;
namespace BT.Models;

public class LoginResponseModel
{
    public UserRequestModel User { get; set; } = new UserRequestModel();
    public string Token { get; set; } = string.Empty;
}