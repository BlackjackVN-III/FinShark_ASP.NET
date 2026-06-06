using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Comment;
using TeddySmith.API.Extensions;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Models;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
       
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(AppDbContext context, ICommentRepository repository, IStockRepository stockRepository
            , UserManager<AppUser> userManager)
        {
            
            _commentRepository = repository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

        var listComment = await _commentRepository.GetAllAsync();

            return Ok(listComment);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cmt = await _commentRepository.GetByIdAsync(id);
            if (cmt == null)
            {
                return BadRequest();
            }
            return Ok(cmt.toCommentDto());
        }


        [HttpPost("{StockId:int}")]
       
        public async Task<IActionResult> Create([FromRoute] int StockId, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _stockRepository.StockExists(StockId))
            {
                return BadRequest();
            }
            // one by one cmt
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var CreateCmt = commentDto.toCommentFromCreate();
            //  gan 
            CreateCmt.AppUserId = appUser.Id;

            await _commentRepository.CreateCommentAsync(CreateCmt);
            return CreatedAtAction(nameof(GetById), new { id = CreateCmt.Id}, CreateCmt.toCommentDto());

        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update( [FromRoute] int id ,[FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateCmt = await _commentRepository.UpdateCommentAsync(commentDto.toCommentFromUdapte(), id);
            if (updateCmt == null)
            {
              return  NotFound("khong co comment nay");
            }
            return Ok(updateCmt.toCommentDto());

            
        }
    } 
}
