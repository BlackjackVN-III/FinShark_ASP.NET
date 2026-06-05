using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Stock;
using TeddySmith.API.Helpers;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Models;

namespace TeddySmith.API.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;
        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
         var delete= await  _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (delete == null)
            {
                return null;
            }
            _context.Stock.Remove(delete);
           await _context.SaveChangesAsync();
            return delete;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            //Sort 

            var stock = _context.Stock.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stock = stock.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                stock = stock.Where(s => s.Symbol.Contains(queryObject.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                if (queryObject.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                {
                    stock = queryObject.isDecsending ? stock.OrderByDescending(s=>s.Symbol) : stock.OrderBy(s=>s.Symbol);
                }

            }

            //Pagination

            var skipNumber = (queryObject.PageNumber -1)* queryObject.PageSize;

                

            return await  stock.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }

        public async Task<StockDto?> GetByIdAsync(int id)
        {
            var stock = await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i=>i.Id ==id);
            return stock?.ToStockDto();

        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
          return  await _context.Stock.FirstOrDefaultAsync(s=>s.Symbol == symbol);

        }

        public async Task<bool> StockExists(int id)
        {
           return await _context.Stock.AnyAsync(x => x.Id == id);
        }

        public async Task<StockDto?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
        {
           var updateModel =  await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
           
            updateModel.Symbol = stockDto.Symbol;
            updateModel.CompanyName = stockDto.CompanyName;
            updateModel.Purchase = stockDto.Purchase;
            updateModel.LastDiv = stockDto.LastDiv;
            updateModel.Industry = stockDto.Industry;
            updateModel.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return updateModel?.ToStockDto();

        }
    }
}
