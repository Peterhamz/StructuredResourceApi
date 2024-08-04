using Microsoft.AspNetCore.Identity;

namespace StructuredResource.Api.Repositories
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
