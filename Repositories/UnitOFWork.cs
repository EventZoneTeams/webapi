﻿using Repositories.Interfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentEventForumDbContext _studentEventForumDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventProductRepository _eventProductRepository;
        private readonly IEventPackageRepository _eventPackageRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IEventFeedbackRepository _eventFeedbackRepository;

        public UnitOfWork(StudentEventForumDbContext studentEventForumDbContext
            , IUserRepository userRepository,
            IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventProductRepository eventProductRepository,
            IEventPackageRepository eventPackageRepository,
            IWalletRepository walletRepository,
            IEventFeedbackRepository eventFeedbackRepository
            )
        {
            _studentEventForumDbContext = studentEventForumDbContext;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventProductRepository = eventProductRepository;
            _eventPackageRepository = eventPackageRepository;
            _walletRepository = walletRepository;
            _eventFeedbackRepository = eventFeedbackRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IEventRepository EventRepository => _eventRepository;
        public IEventCategoryRepository EventCategoryRepository => _eventCategoryRepository;
        public IEventProductRepository EventProductRepository => _eventProductRepository;
        public IEventPackageRepository EventPackageRepository => _eventPackageRepository;
        public IWalletRepository WalletRepository => _walletRepository;

        public IEventFeedbackRepository EventFeedbackRepository => _eventFeedbackRepository;

        public Task<int> SaveChangeAsync()
        {
            return _studentEventForumDbContext.SaveChangesAsync();
        }
    }
}