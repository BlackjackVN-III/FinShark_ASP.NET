using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Stock;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Repository;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStockRepository _stockRepository;
        public StockController(AppDbContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepository.GetAllAsync();
            
            return Ok(stocks); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stocks = await _stockRepository.GetByIdAsync(id);

            if (stocks == null)
            {
                return NotFound();
            }
            return Ok(stocks);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = await _stockRepository.CreateStockAsync(stockDto);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel);

        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var updateModel = await _stockRepository.UpdateStockAsync(id, updateDto);
            if (updateModel == null)
            {
                return NotFound();
            }
            return Ok(updateModel);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleteModel = await _stockRepository.DeleteStockAsync(id);
            if (deleteModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
