using System.Security.Claims;

namespace ContactsWebApi.Helpers
{
    public interface ITokenProvider
    {
        string GenerateToken(IList<Claim> claims);
        string GenerateToken(string email, string? userId, string? role);
    }
}