namespace LoanProcessing.Application.Authentication.Commands.Login;
public class LoginResponse
{
    public string Token { get; set; }
    public string Username { get; set; }
    public long UserId { get; set; }
}
