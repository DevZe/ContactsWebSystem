
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ContactsApp.Helpers
{

    public class JwtParser
    {
        public static IEnumerable<Claim>? ParseJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            IEnumerable<Claim> claims = new List<Claim>();
            foreach (var claim in jwtToken.Claims)
            {
                claims.Append(claim);
            }
            return claims;
        }
    }
}
