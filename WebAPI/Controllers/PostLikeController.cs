using EventZone.Domain.DTOs.PostLikeComments;
using EventZone.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventZone.Repositories.Commons;
using EventZone.Domain.DTOs.PostCommentDTOs;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        /// <summary>
        /// Like a post
        /// </summary>
        /// <param name="postId">The ID of the post to like</param>
        /// <returns>The created PostLike</returns>
        [HttpPost("posts/{postId}/like")]
        public async Task<ActionResult> LikePost(Guid postId)
        {
            try
            {
                var result = await _postLikeService.CreatePostLikeAsync(postId);
                return Ok(ApiResult<PostLikeDTO>.Succeed(result, "Post liked successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get all likes for a specific post
        /// </summary>
        /// <param name="postId">The ID of the post</param>
        /// <returns>List of PostLikes</returns>
        [HttpGet("posts/{postId}/likes")]
        public async Task<ActionResult> GetLikesForPost(Guid postId)
        {
            try
            {
                var likes = await _postLikeService.GetAllLikesForPostAsync(postId);
                if (likes == null || likes.Count == 0)
                {
                    return NotFound(ApiResult<PostLikeDTO>.Error(null, "No likes found for post id: " + postId));
                }
                return Ok(ApiResult<List<PostLikeDTO>>.Succeed(likes, "Likes retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get comments for a liked post
        /// </summary>
        /// <param name="postId">The ID of the post</param>
        /// <returns>List of comments for the liked post</returns>
        [HttpGet("posts/{postId}/liked-post-comments")]
        public async Task<ActionResult> GetCommentsForLikedPost(Guid postId)
        {
            try
            {
                var comments = await _postLikeService.GetCommentsForLikedPostAsync(postId);
                if (comments == null || comments.Count == 0)
                {
                    return NotFound(ApiResult<object>.Error(null, "No comments found for post id: " + postId));
                }
                return Ok(ApiResult<List<PostCommentDTO>>.Succeed(comments, "Comments retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get a specific like by its ID
        /// </summary>
        /// <param name="likeId">The ID of the like</param>
        /// <returns>The details of the like</returns>
        [HttpGet("post-likes/{likeId}")]
        public async Task<ActionResult> GetPostLikeById(Guid likeId)
        {
            try
            {
                var like = await _postLikeService.GetPostLikeByIdAsync(likeId);
                if (like == null)
                {
                    return NotFound(ApiResult<PostLikeDTO>.Error(null, "Like not found with id: " + likeId));
                }
                return Ok(ApiResult<PostLikeDTO>.Succeed(like, "Like retrieved successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Update a specific like
        /// </summary>
        /// <param name="likeId">The ID of the like</param>
        /// <param name="postLikeUpdateDTO">Updated details of the like</param>
        /// <returns>The updated PostLike</returns>
        [HttpPut("post-likes/{likeId}")]
        public async Task<ActionResult> UpdatePostLike(Guid likeId, PostLikeUpdateDTO postLikeUpdateDTO)
        {
            try
            {
                var result = await _postLikeService.UpdatePostLikeAsync(likeId, postLikeUpdateDTO);
                if (result == null)
                {
                    return NotFound(ApiResult<PostLikeDTO>.Error(null, "Like not found with id: " + likeId));
                }
                return Ok(ApiResult<PostLikeDTO>.Succeed(result, "Like updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Delete a like by its ID (soft delete)
        /// </summary>
        /// <param name="likeId">The ID of the like</param>
        /// <returns>Status of the deletion</returns>
        [HttpDelete("post-likes/{likeId}")]
        public async Task<ActionResult> DeletePostLike(Guid likeId)
        {
            try
            {
                var result = await _postLikeService.DeletePostLikeAsync(likeId);
                if (!result)
                {
                    return NotFound(ApiResult<PostLikeDTO>.Error(null, "Like not found with id: " + likeId));
                }
                return Ok(ApiResult<bool>.Succeed(result, "Like deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}