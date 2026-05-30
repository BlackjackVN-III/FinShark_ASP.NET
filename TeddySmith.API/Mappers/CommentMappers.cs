using Microsoft.Identity.Client;
using TeddySmith.API.DTOs.Comment;
using TeddySmith.API.Models;

namespace TeddySmith.API.Mappers
{
    public static class CommentMappers
    {

        public static CommentDto toCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreateOn = commentModel.CreateOn,
                StockId = commentModel.StockId
            };
        }

        public static Comment toCommentFromCreate(this CreateCommentDto commentDto)
        {
            return new Comment
            {

                Title = commentDto.Title,
                Content = commentDto.Content
               


            };
        }
        public static Comment toCommentFromUdapte(this UpdateCommentDto commentDto )
        {
            return new Comment
            {

                Title = commentDto.Title,
                Content = commentDto.Content,
             
               


            };
        }

    }


    }

