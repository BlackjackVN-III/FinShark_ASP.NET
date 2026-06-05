using System.Runtime.CompilerServices;
using TeddySmith.API.DTOs.Stock;
using TeddySmith.API.Helpers;
using TeddySmith.API.Models;

namespace TeddySmith.API.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject queryObject);
        Task<StockDto?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync (string  symbol)  ;
        Task<Stock> CreateStockAsync(CreateStockRequestDto stockDto);
        Task<StockDto?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteStockAsync(int id);
        Task<bool> StockExists(int id);
    }
}