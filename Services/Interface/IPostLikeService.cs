using EventZone.Domain.DTOs.PostCommentDTOs;
using EventZone.Domain.DTOs.PostLikeComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Interface
{
    public interface IPostLikeService
    {
        Task<PostLikeDTO> CreatePostLikeAsync(Guid postId);

        Task<bool> DeletePostLikeAsync(Guid likeId);

        Task<List<PostLikeDTO>> GetAllLikesForPostAsync(Guid postId);

        Task<List<PostCommentDTO>> GetCommentsForLikedPostAsync(Guid postId);

        Task<PostLikeDTO> GetPostLikeByIdAsync(Guid likeId);

        Task<PostLikeDTO> UpdatePostLikeAsync(Guid likeId, PostLikeUpdateDTO updatedLikeDTO);
    }
}