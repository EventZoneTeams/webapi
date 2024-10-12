using EventZone.Domain.DTOs.PostCommentDTOs;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventZone.WebAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class EventPostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public EventPostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("event-posts")]
        public async Task<ActionResult> CreatePost(PostDTO post)
        {
            try
            {
                var result = await _postService.CreateNewPostAsync(post);
                return Ok(ApiResult<PostDetailDTO>.Succeed(result, "Post created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get list of posts by eventId
        /// </summary>
        /// <response code="200">Returns a list of posts</response>
        /// <response code="400">Error during reading data</response>
        /// <response code="404">Event Id is not exist</response>
        [HttpGet("events/{id}/event-posts")]
        public async Task<ActionResult> GetPostsByEventId(Guid id)
        {
            try
            {
                var posts = await _postService.GetPostsByEventIdAsync(id);
                if (posts == null)
                {
                    return NotFound(ApiResult<PostDetailDTO>.Error(null, "No posts found for event id: " + id));
                }
                return Ok(ApiResult<List<PostDetailDTO>>.Succeed(posts, "Retrieved posts successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get list of posts by eventId
        /// </summary>
        /// <response code="200">Returns a list of posts</response>
        /// <response code="400">Error during reading data</response>
        /// <response code="404">Event Id is not exist</response>
        [HttpGet("event-posts")]
        public async Task<ActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();

                return Ok(ApiResult<List<PostDetailDTO>>.Succeed(posts, "Retrieved posts successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Get a post by Id
        /// </summary>
        /// <response code="200">Returns a post</response>
        [HttpGet("event-posts/{id}")]
        public async Task<IActionResult> GetPostByIdAsync(Guid id)
        {
            try
            {
                var result = await _postService.GetPostByIdAsync(id);
                if (result != null)
                {
                    return Ok(ApiResult<PostDetailDTO>.Succeed(result, "Post retrieved successfully"));
                }
                return NotFound(ApiResult<PostDetailDTO>.Error(null, "No post found with id: " + id));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Delete a post by Id (soft delete)
        /// </summary>
        /// <response code="200">Returns the removed post</response>
        [HttpDelete("event-posts/{id}")]
        public async Task<IActionResult> DeletePostAsync(Guid id)
        {
            try
            {
                var result = await _postService.DeletePostByIdAsync(id);
                if (result)
                {
                    return Ok(ApiResult<bool>.Succeed(result, "Post deleted successfully"));
                }
                return NotFound(ApiResult<PostDetailDTO>.Error(null, "No post found with id: " + id));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }

        /// <summary>
        /// Update a post by Id
        /// </summary>
        /// <response code="200">Returns the updated post</response>
        [HttpPut("event-posts/{id}")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] Guid id, [FromBody] PostUpdateDTO model)
        {
            try
            {
                var result = await _postService.UpdatePostAsync(id, model);
                if (result == null)
                {
                    return NotFound(ApiResult<PostDTO>.Error(null, "No post found with id: " + id));
                }

                return Ok(ApiResult<PostDTO>.Succeed(result, "Post updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<object>.Fail(ex));
            }
        }
    }
}