using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeddySmith.API.Extensions;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Models;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortpolioRepository _portpolioRepository;
        public FortfolioController(UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortpolioRepository portpolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portpolioRepository = portpolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortpolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortpolio = await _portpolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortpolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePortpolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Stock not found");
            }
            var userPort = await _portpolioRepository.GetUserPortfolio(appUser);
            if (userPort.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Cannot add same stock to portpolio");

            };

            var portModel = new Portfollo
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portpolioRepository.CreateAsync(portModel);
            if (portModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }

        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPort = await _portpolioRepository.GetUserPortfolio(appUser);

        var filterStock = userPort.Where(s=> s.Symbol.ToLower()== symbol.ToLower()).ToList();

            if(filterStock.Count() == 1)
            {
                await _portpolioRepository.DeleteAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portpolio");
            }
            return Ok();
        }
    }
}
