using EventZone.Domain.DTOs.PostCommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Interfaces
{
    public interface IPostService
    {
        Task<PostDetailDTO> CreateNewPostAsync(PostDTO createPost);

        Task<bool> DeletePostByIdAsync(Guid postId);

        Task<List<PostDetailDTO>> GetAllPostsAsync();

        Task<PostDetailDTO> GetPostByIdAsync(Guid postId);

        Task<List<PostDetailDTO>> GetPostsByEventIdAsync(Guid eventId);

        Task<PostDetailDTO> UpdatePostAsync(Guid postId, PostUpdateDTO updatePost);
    }
}