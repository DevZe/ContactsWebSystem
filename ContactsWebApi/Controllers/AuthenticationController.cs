using ContactsWebApi.Helpers;
using ContactsWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthenticationController(SignInManager<IdentityUser> signInManager,
            ITokenProvider tokenProvider, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _config = config;
            _tokenProvider = tokenProvider;
            _userManager = userManager;

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
        {
            try
            {
                Microsoft.AspNetCore.Identity.SignInResult result = new Microsoft.AspNetCore.Identity.SignInResult();
                //Authenticate the user using Identity - Signin Manager
                //This to log in the user
                if (loginModel == null)
                {
                    return BadRequest("Error login in please try again");
                }

                result = await _signInManager.PasswordSignInAsync(userName: loginModel.Email, loginModel.Password, isPersistent: true, true);

                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(loginModel.Email ?? _signInManager.Context.User.Identity.Name);

                    if (user != null)
                    {
                        // var claims = (await _userManager.GetClaimsAsync(user)).ToList();
                        // Generate token using JWT
                        var token = _tokenProvider.GenerateToken(loginModel.Email ?? user.Email, loginModel.Password ?? user.Id, loginModel.Password);
                        // Set the token as the authentication token for the user
                        var identityResult = await _signInManager.UserManager.SetAuthenticationTokenAsync(user, "Jwt", "Bearer", token);
                        return Ok(token);
                    }
                    return Accepted();
                }
                else if (result.IsLockedOut)
                {

                    return StatusCode(423);
                }
                else if (result.IsNotAllowed)
                {

                    return Unauthorized();
                }
                else
                {

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
        {
            var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

    }
}
