using AutoMapper;
using EventZone.Domain.DTOs.PostCommentDTOs;
using EventZone.Domain.DTOs.PostLikeComments;
using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using EventZone.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Services.Services
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostLikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create a new like for a post
        public async Task<PostLikeDTO> CreatePostLikeAsync(Guid postId)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }
            var user = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (user == null)
            {
                throw new Exception("User not existing");
            }

            var newPostLike = new PostLike
            {
                UserId = user.Id,
                PostId = postId,
            };
            var result = await _unitOfWork.PostLikeRepository.AddAsync(newPostLike);
            var saveResult = await _unitOfWork.SaveChangeAsync();

            if (saveResult <= 0)
            {
                throw new Exception("Error while liking the post");
            }

            return _mapper.Map<PostLikeDTO>(result);
        }

        // Get all likes for a post
        public async Task<List<PostLikeDTO>> GetAllLikesForPostAsync(Guid postId)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }

            var likes = await _unitOfWork.PostLikeRepository.GetAllAsync(x => x.PostId == postId);
            return _mapper.Map<List<PostLikeDTO>>(likes);
        }

        // Get comments for the post that is liked
        public async Task<List<PostCommentDTO>> GetCommentsForLikedPostAsync(Guid postId)
        {
            // Kiểm tra sự tồn tại của post
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, x => x.PostComments);
            if (post == null)
            {
                throw new Exception("Post does not exist");
            }

            // Trả về danh sách các comment cho post đó
            return _mapper.Map<List<PostCommentDTO>>(post.PostComments);
        }

        // Get a like by its Id
        public async Task<PostLikeDTO> GetPostLikeByIdAsync(Guid likeId)
        {
            var postLike = await _unitOfWork.PostLikeRepository.GetByIdAsync(likeId);
            if (postLike == null)
            {
                throw new Exception("Like does not exist");
            }

            return _mapper.Map<PostLikeDTO>(postLike);
        }

        // Update a like
        public async Task<PostLikeDTO> UpdatePostLikeAsync(Guid likeId, PostLikeUpdateDTO updatedLikeDTO)
        {
            var existingLike = await _unitOfWork.PostLikeRepository.GetByIdAsync(likeId);
            if (existingLike == null)
            {
                throw new Exception("Like does not exist");
            }

            existingLike = _mapper.Map(updatedLikeDTO, existingLike);
            await _unitOfWork.PostLikeRepository.Update(existingLike);
            var saveResult = await _unitOfWork.SaveChangeAsync();

            if (saveResult <= 0)
            {
                throw new Exception("Error while updating the like");
            }

            return _mapper.Map<PostLikeDTO>(existingLike);
        }

        // Delete a like by its Id (Soft delete)
        public async Task<bool> DeletePostLikeAsync(Guid likeId)
        {
            var postLike = await _unitOfWork.PostLikeRepository.GetByIdAsync(likeId);
            if (postLike == null)
            {
                throw new Exception("Like does not exist");
            }

            await _unitOfWork.PostLikeRepository.SoftRemove(postLike);
            var saveResult = await _unitOfWork.SaveChangeAsync();

            if (saveResult > 0)
            {
                return true;
            }

            throw new Exception("Error while deleting the like");
        }
    }
}