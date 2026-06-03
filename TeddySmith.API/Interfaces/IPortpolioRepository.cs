using TeddySmith.API.Models;

namespace TeddySmith.API.Interfaces
{
    public interface IPortpolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
    }
}
