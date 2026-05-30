using TeddySmith.API.DTOs.Comment;
using TeddySmith.API.Models;

namespace TeddySmith.API.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<CommentDto>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id) ;
        Task<Comment?> CreateCommentAsync(Comment commentModel);
        Task<Comment?> UpdateCommentAsync(Comment commentModel, int id);
        
    }
}
