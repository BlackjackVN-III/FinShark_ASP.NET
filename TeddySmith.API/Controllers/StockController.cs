using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Stock;
using TeddySmith.API.Helpers;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Repository;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
       
        private readonly IStockRepository _stockRepository;
        public StockController( IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
           
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _stockRepository.GetAllAsync(queryObject);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks); 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetByIdAsync(id);

            if (stocks == null)
            {
                return NotFound();
            }
            return Ok(stocks);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.CreateStockAsync(stockDto);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel);

        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateModel = await _stockRepository.UpdateStockAsync(id, updateDto);
            if (updateModel == null)
            {
                return NotFound();
            }
            return Ok(updateModel);

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleteModel = await _stockRepository.DeleteStockAsync(id);
            if (deleteModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
