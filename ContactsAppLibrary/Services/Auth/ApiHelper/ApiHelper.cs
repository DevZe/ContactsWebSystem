using ContactsAppLibrary.Services.Models.Auth;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ContactsAppLibrary.Services.Auth.ApiHelper
{
    public class ApiHelper : IApiHelper
    {
        //readonly IHttpClientFactory _clientFactory;
        private HttpClient _apiClient;
        private readonly IConfiguration _Configuration;

        public ApiHelper(IConfiguration configuration)
        {
            _Configuration = configuration;
            InitializeClient();
        }

        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }


        //We initialize the HTTP client and format the clients headings to pass the data as a json objet
        private void InitializeClient()
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(_Configuration["ApiUrl"] ?? "");
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/*+json"));
        }

        //Overloading the initializeClient method to pass the token as a parameter
        public void InitializeClient(string token)
        {
            InitializeClient();
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        /// <summary>
        ///authenticate the user by passing a username and password to the token endpoint and return a authenticate user with a token
        /// </summary>
        /// <returns>logged in user model</returns>
        public async Task<string> AuthenticateUser(string username, string password)
        {

            LoginModel? data = new LoginModel
            {
                Email = username,
                Password = password
            };

            using (HttpResponseMessage httpResponse = await _apiClient.PostAsJsonAsync("api/Authentication/login", data))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadAsStringAsync();

                    return response;
                }
                else
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
            }

        }


        //Log the user out of the system ***cheat!
        //public async Task<bool> LogOutuser()
        //{

        //    try
        //    {
        //        var response = await _apiClient.PostAsJsonAsync("auth/logout", new { });
        //        _apiClient.DefaultRequestHeaders.Clear();
        //        return response.IsSuccessStatusCode;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
