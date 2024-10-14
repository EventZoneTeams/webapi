using EventZone.Domain.Entities;
using EventZone.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly StudentEventForumDbContext _context;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public PostRepository(StudentEventForumDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public async Task<List<EventImage>> AddImagesStringForProduct(Guid postId, List<string> images)
        {
            try
            {
                var post = await GetByIdAsync(postId);
                if (post == null)
                {
                    throw new Exception("Invalid post is not existing in database");
                }
                var newImages = new List<EventImage>();
                foreach (var image in images)
                {
                    if (image == null)
                    {
                        continue;
                    }
                    newImages.Add(new EventImage()
                    {
                        EventId = post.EventId,
                        ImageUrl = image,
                        Name = image,
                        PostId = postId
                    });
                }

                await _context.AddRangeAsync(newImages);
                return newImages;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}