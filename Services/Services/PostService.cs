using AutoMapper;
using EventZone.Domain.DTOs.ImageDTOs;
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
            try
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
                    EventImages = []
                };

                var result = await _unitOfWork.PostRepository.AddAsync(newPost);
                var check = await _unitOfWork.SaveChangeAsync();

                if (check <= 0)
                {
                    throw new Exception("Post created failed in process");
                }
                var imagerResult = await _unitOfWork.PostRepository.AddImagesStringForProduct(result.Id, createPost.ImageUrls);
                check = await _unitOfWork.SaveChangeAsync();
                result.EventImages = imagerResult;

                return _mapper.Map<PostDetailDTO>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get all Posts
        public async Task<List<PostDetailDTO>> GetAllPostsAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync(x => x.Event, x => x.EventImages);
            return _mapper.Map<List<PostDetailDTO>>(posts);
        }

        // Get all posts by EventId
        public async Task<List<PostDetailDTO>> GetPostsByEventIdAsync(Guid eventId)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetByIdAsync(eventId);
            if (existingEvent == null)
            {
                return null;
            }

            var posts = await _unitOfWork.PostRepository.GetAllAsync( x => x.EventImages);
            return _mapper.Map<List<PostDetailDTO>>(posts.Where(x=> x.EventId == eventId));
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
            var existingPost = await _unitOfWork.PostRepository.GetByIdAsync(postId, x => x.EventImages);
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

                if (!updatePost.ImageUrls.IsNullOrEmpty())
                {
                    existingPost.EventImages.ToList().ForEach(item => item.IsDeleted = true);

                    foreach (var item in updatePost.ImageUrls)
                    {
                        var tmp = await _unitOfWork.EventProductRepository.GetProductImageByUrl(item);
                        if (tmp == null)
                        {
                            var image = new EventImage
                            {
                                ImageUrl = item,
                                Name = item
                            };
                            existingPost.EventImages.Add(image);
                        }
                        else
                        {
                            tmp.IsDeleted = false;
                        }
                    }
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
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, x => x.EventImages);
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