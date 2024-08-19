using Blazored.SessionStorage;
using ContactsAppLibrary.Services.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace ContactsApp.Helpers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticatedUserModel _authuserModel;
        private IConfiguration _config;
        string secret = string.Empty;
        private readonly ISessionStorageService _sessionStorageService;

        public CustomAuthenticationStateProvider(AuthenticatedUserModel authenticatedUser, IConfiguration config, ISessionStorageService sessionStorageService)
        {
            _authuserModel = authenticatedUser;
            _config = config;
            secret = _config.GetValue("JWTSecretKey", "");
            _sessionStorageService = sessionStorageService;

        }

        //Get the Authentication state of the token 
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var identity = new ClaimsIdentity();
            if (!string.IsNullOrEmpty(_authuserModel.accessToken))
            {

                identity = new ClaimsIdentity(JwtParser.ParseJwtToken(_authuserModel.accessToken), "jwt");
            }
            else
            {
                var token = await _sessionStorageService.GetItemAsync<string>("token");
                identity = new ClaimsIdentity(JwtParser.ParseJwtToken(token), "jwt");
            }
            var user = new ClaimsPrincipal(identity);
            AuthenticationState? state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
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