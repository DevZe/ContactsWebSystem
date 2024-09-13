using ContactsAppLibrary.Services.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ContactsApp.Helpers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticatedUserModel _authuserModel;
        private IConfiguration _config;
        string token = string.Empty;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(AuthenticatedUserModel authenticatedUser, IConfiguration config, ILocalStorageService localStorage)
        {
            _authuserModel = authenticatedUser;
            _config = config;
            _localStorage = localStorage;

        }

        //Get the Authentication state of the token 
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var identity = new ClaimsIdentity();

            token = _authuserModel.accessToken;
            if (token == null)
            {
                token = await _localStorage.GetItem<string>("token");
                _authuserModel.accessToken = token;
            }
            else
            {
                await _localStorage.SetItem<string>("token", token);
            }

            if (token != null)
            {
                identity = new ClaimsIdentity(JwtParser.ParseJwtToken(_authuserModel.accessToken ?? token), "jwt");
                //await _localStorage.SetItem<string>("token", token);


            }
            var user = new ClaimsPrincipal(identity);
            AuthenticationState? state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }



        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }

}