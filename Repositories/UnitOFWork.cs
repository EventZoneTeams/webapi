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
        private readonly IEventPackageRepository _eventPackageRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventOrderRepository _eventOrderRepository;
        private readonly IEventFeedbackRepository _eventFeedbackRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventCampaignRepository _eventCampaignRepository;

        public UnitOfWork(StudentEventForumDbContext studentEventForumDbContext
            , IUserRepository userRepository,
            IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventProductRepository eventProductRepository,
            IEventPackageRepository eventPackageRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IEventOrderRepository eventOrderRepository,
            IEventFeedbackRepository eventFeedbackRepository,
            INotificationRepository notificationRepository,
            IEventCampaignRepository eventCampaignRepository
            )
        {
            _studentEventForumDbContext = studentEventForumDbContext;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventProductRepository = eventProductRepository;
            _eventPackageRepository = eventPackageRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _eventOrderRepository = eventOrderRepository;
            _eventFeedbackRepository = eventFeedbackRepository;
            _notificationRepository = notificationRepository;
            _eventCampaignRepository = eventCampaignRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IEventRepository EventRepository => _eventRepository;
        public IEventCategoryRepository EventCategoryRepository => _eventCategoryRepository;
        public IEventProductRepository EventProductRepository => _eventProductRepository;
        public IEventPackageRepository EventPackageRepository => _eventPackageRepository;
        public IWalletRepository WalletRepository => _walletRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IEventOrderRepository EventOrderRepository => _eventOrderRepository;
        public IEventFeedbackRepository EventFeedbackRepository => _eventFeedbackRepository;
        public INotificationRepository NotificationRepository => _notificationRepository;
        public IEventCampaignRepository EventCampaignRepository => _eventCampaignRepository;

        public Task<int> SaveChangeAsync()
        {
            return _studentEventForumDbContext.SaveChangesAsync();
        }
    }
}