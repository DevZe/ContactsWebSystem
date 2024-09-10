namespace ContactsAppLibrary.Services.Auth.ApiHelper
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<string> AuthenticateUser(string username, string password);

        void InitializeClient(string token);
        Task<bool> LogOutUser();
        //Task<bool> LogOutuser();
    }
}