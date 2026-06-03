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
    }
}
