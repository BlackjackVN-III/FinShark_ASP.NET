using Microsoft.EntityFrameworkCore;
using TeddySmith.API.Data;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Models;

namespace TeddySmith.API.Repository
{
    public class PortfolioRepository : IPortpolioRepository
    {
        private readonly AppDbContext _context;
        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfollos.Where(u => u.AppUserId == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap


                }).ToListAsync();
        }
    }
}
