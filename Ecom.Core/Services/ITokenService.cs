using Ecom.Core.Entities;

namespace Ecom.Core.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}


