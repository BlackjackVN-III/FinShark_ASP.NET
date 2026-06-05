using TeddySmith.API.Models;

namespace TeddySmith.API.Interfaces
{
    public interface IPortpolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfollo> CreateAsync(Portfollo portfollo);
        Task<Portfollo> DeleteAsync(AppUser appUser,string symbol);
    }
}
