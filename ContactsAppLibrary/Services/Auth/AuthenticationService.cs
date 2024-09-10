using ContactsAppLibrary.Services.Auth.ApiHelper;
using ContactsAppLibrary.Services.Models.Auth;
using System.Net.Http.Json;

namespace ContactsAppLibrary.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        //Local Variables for the inherited classes
        readonly IApiHelper _apiHelper;
        private AuthenticatedUserModel _authedUser;


        public AuthenticationService(IApiHelper apiHelper, AuthenticatedUserModel authedUser)
        {
            _apiHelper = apiHelper;
            _authedUser = authedUser;

        }


        /// <summary>
        /// User is logging here using Jwt token as a return type
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns>JwtToken</returns>
        /// <exception cref="Exception"></exception>
        public async Task<AuthenticatedUserModel> LoginUser(LoginModel loginModel)
        {
            try
            {

                if (loginModel.Email != null || loginModel.Password != null)
                {
                    //authenticate and get the Authenticated user model
                    string? result = await _apiHelper.AuthenticateUser(loginModel.Email, loginModel.Password);
                    if (result != null)
                        _authedUser.accessToken = result;//our precious Jwt Token ;)

                }

                return _authedUser;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }




        //Log the user out
        public async Task<bool> LogOut()
        {



            if (_authedUser == null) { return true; }
            if (_authedUser != null && _authedUser.accessToken != null)
            {

                //    _apiHelper.InitializeClient(_authedUser.accessToken);

                //remove the token
                await _apiHelper.LogOutUser();
                //release variables
                _authedUser.accessToken = null;
                _authedUser = new AuthenticatedUserModel();
                return true;

            }
            else { return false; }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Register(RegisterModel registerModel)
        {
            try
            {

                if (registerModel.Email == null || registerModel.Password == null)
                {
                    return false;
                }
                //API Call to auth controller in register end point
                var response = await _apiHelper.ApiClient.PostAsJsonAsync<RegisterModel>("api/Authentication/register", registerModel);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Confirming the email address of the newly added user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public async Task<bool> ConfirmEmail(string userId, string? code)
        //{
        //    try
        //    {
        //        //Null check
        //        if (userId == null || code == null) { return false; }
        //        var response = await _apiHelper.ApiClient.PostAsJsonAsync($"auth/confirmemail?userId={userId}&code={code}", new { });
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
