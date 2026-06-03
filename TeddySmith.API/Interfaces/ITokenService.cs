using TeddySmith.API.Models;

namespace TeddySmith.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
