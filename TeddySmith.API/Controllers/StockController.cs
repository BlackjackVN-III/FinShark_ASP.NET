using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Stock;
using TeddySmith.API.Mappers;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stock
                .Select(s => s.ToStockDto()).ToListAsync();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stocks = await _context.Stock.FindAsync(id);

            if (stocks == null)
            {
                return NotFound();
            }
            return Ok(stocks.ToStockDto());

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());

        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var updateModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (updateModel == null)
            {
                return NotFound();
            }
            updateModel.Symbol = updateDto.Symbol;
            updateModel.CompanyName = updateDto.CompanyName;
            updateModel.Purchase = updateDto.Purchase;
            updateModel.LastDiv = updateDto.LastDiv;
            updateModel.Industry = updateDto.Industry;
            updateModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(updateModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(deleteModel);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
