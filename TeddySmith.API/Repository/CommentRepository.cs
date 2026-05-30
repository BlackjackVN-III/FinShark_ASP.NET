using Microsoft.EntityFrameworkCore;
using TeddySmith.API.Data;
using TeddySmith.API.DTOs.Comment;
using TeddySmith.API.Interfaces;
using TeddySmith.API.Mappers;
using TeddySmith.API.Models;

namespace TeddySmith.API.Repository
{
    public class CommentRepository : ICommentRepository
    {
        
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
           await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<CommentDto>> GetAllAsync()
        {
            return await _context.Comments.Select(x =>x.toCommentDto()).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> UpdateCommentAsync(Comment commentModel , int commentId)
        {
           var updatedCmt =  await _context.Comments.FirstOrDefaultAsync(z => z.Id == commentId);

            if (updatedCmt == null)
            {
                return null;
            }
            updatedCmt.Title = commentModel.Title;
            updatedCmt.Content = commentModel.Content;
            
            await _context.SaveChangesAsync();
            return updatedCmt;

        }

       
    }
}

