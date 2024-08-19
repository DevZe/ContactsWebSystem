namespace ContactsAppLibrary.Services.Models.Auth
{
    public class AuthenticatedUserModel
    {

        public string? accessToken { get; set; }
        public string? UserName { get; set; }
        public string? tokenType { get; set; }

    }
}
