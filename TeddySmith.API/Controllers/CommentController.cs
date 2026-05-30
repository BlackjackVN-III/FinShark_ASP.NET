using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Comment;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Models;

namespace TeddySmith.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(AppDbContext context, ICommentRepository repository, IStockRepository stockRepository)
        {
            _context = context;
            _commentRepository = repository;
            _stockRepository = stockRepository;
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
            var CreateCmt = commentDto.toCommentFromCreate();
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
