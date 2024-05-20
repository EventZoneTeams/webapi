using Repositories.Interfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public UnitOfWork(StudentEventForumDbContext studentEventForumDbContext
            , IUserRepository userRepository,
            IEventRepository eventRepository
            )
        {
            _studentEventForumDbContext = studentEventForumDbContext;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IEventRepository EventRepository => _eventRepository;

        public Task<int> SaveChangeAsync()
        {
            return _studentEventForumDbContext.SaveChangesAsync();
        }
    }
}
