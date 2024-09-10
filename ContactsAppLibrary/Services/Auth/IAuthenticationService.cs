using ContactsAppLibrary.Services.Models.Auth;

namespace ContactsAppLibrary.Services.Auth
{
    public interface IAuthenticationService
    {
        //Task<bool> ConfirmEmail(string userId, string? code);
        Task<AuthenticatedUserModel> LoginUser(LoginModel loginModel);
        //Task<AuthenticatedUserModel> LoginUser(string? userId);
        Task<bool> Register(RegisterModel registerModel);
        Task<bool> LogOut();
    }
}