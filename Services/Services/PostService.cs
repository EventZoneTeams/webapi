using AutoMapper;
using EventZone.Domain.DTOs.PostCommentDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostDetailDTO> CreateNewPostAsync(PostDTO createPost)
        {
            // Check if the event exists
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(createPost.EventId);
            if (existingEvent == null)
            {
                throw new Exception("Event does not exist");
            }

            var newPost = new Post
            {
                EventId = existingEvent.Id,
                Title = createPost.Title,
                Body = createPost.Body,
            };

            var result = await _unitOfWork.PostRepository.AddAsync(newPost);
            var check = await _unitOfWork.SaveChangeAsync();

            if (check > 0)
            {
                return _mapper.Map<PostDetailDTO>(result);
            }
            else
            {
                throw new Exception("Post creation failed");
            }
        }

        // Get all Posts
        public async Task<List<PostDetailDTO>> GetAllPostsAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            return _mapper.Map<List<PostDetailDTO>>(posts);
        }

        // Get all posts by EventId
        public async Task<List<PostDetailDTO>> GetPostsByEventIdAsync(Guid eventId)
        {
            if (await _unitOfWork.EventRepository.GetByIdAsync(eventId) == null)
            {
                return null;
            }

            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            return _mapper.Map<List<PostDetailDTO>>(posts.FindAll(p => p.EventId == eventId));
        }

        public async Task<PostDetailDTO> GetPostByIdAsync(Guid postId)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return null; // khi gọi từ controller nếu trả về null sẽ quăng ra 404 not found api result
            }

            return _mapper.Map<PostDetailDTO>(post);
        }

        // Update an existing Post
        public async Task<PostDetailDTO> UpdatePostAsync(Guid postId, PostUpdateDTO updatePost)
        {
            var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (existingPost != null)
            {
                if (!updatePost.Title.IsNullOrEmpty())
                {
                    existingPost.Title = updatePost.Title;
                }

                if (!updatePost.Body.IsNullOrEmpty())
                {
                    existingPost.Body = updatePost.Body;
                }

                await _unitOfWork.PostRepository.Update(existingPost);
                var updatedResult = await _unitOfWork.SaveChangeAsync();

                if (updatedResult > 0)
                {
                    return _mapper.Map<PostDetailDTO>(existingPost);
                }
                else
                {
                    throw new Exception("Post update failed");
                }
            }
            else
            {
                throw new Exception("Post not found");
            }
        }

        // Delete Post by Id (Soft delete)
        public async Task<bool> DeletePostByIdAsync(Guid postId)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post != null)
            {
                await _unitOfWork.PostRepository.SoftRemove(post);
                var result = await _unitOfWork.SaveChangeAsync();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Failed to delete post");
                }
            }

            throw new Exception("Post not found");
        }
    }
}