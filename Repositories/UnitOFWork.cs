using Repositories.Interfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventProductRepository _eventProductRepository;


        public UnitOfWork(StudentEventForumDbContext studentEventForumDbContext
            , IUserRepository userRepository,
            IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventProductRepository eventProductRepository
            )
        {
            _studentEventForumDbContext = studentEventForumDbContext;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventProductRepository = eventProductRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IEventRepository EventRepository => _eventRepository;
        public IEventCategoryRepository EventCategoryRepository => _eventCategoryRepository;

        public IEventProductRepository EventProductRepository => _eventProductRepository;



        public Task<int> SaveChangeAsync()
        {
            return _studentEventForumDbContext.SaveChangesAsync();
        }
    }
}
