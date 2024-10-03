using EventZone.Repositories.Interfaces;

namespace EventZone.Repositories
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
        private readonly IEventDonationRepository _eventDonationRepository;
        private readonly IEventTicketRepository _eventTicketRepository;
        private readonly IEventBoardRepository _eventBoardRepository;
        private readonly IEventBoardLabelRepository _eventBoardLabelRepository;
        private readonly IEventBoardTaskLabelRepository _eventBoardTaskLabelRepository;
        private readonly IEventBoardColumnRepository _eventBoardColumnRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IEventBoardTaskRepository _eventBoardTaskRepository;
        private readonly IProductImageRepository _productImageRepository;

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
            IEventCampaignRepository eventCampaignRepository,
            IEventDonationRepository eventDonationRepository,
            IEventTicketRepository eventTicketRepository,
            IEventBoardRepository eventBoardRepository,
            IEventBoardLabelRepository eventBoardLabelRepository,
            IEventBoardTaskLabelRepository eventBoardTaskLabelRepository,
            IEventBoardColumnRepository eventBoardColumnRepository,
            IAttendeeRepository attendeeRepository,
            IEventBoardTaskRepository eventBoardTaskRepository,
            IProductImageRepository productImageRepository
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
            _eventDonationRepository = eventDonationRepository;
            _eventTicketRepository = eventTicketRepository;
            _eventBoardRepository = eventBoardRepository;
            _eventBoardLabelRepository = eventBoardLabelRepository;
            _eventBoardTaskLabelRepository = eventBoardTaskLabelRepository;
            _eventBoardColumnRepository = eventBoardColumnRepository;
            _attendeeRepository = attendeeRepository;
            _eventBoardTaskRepository = eventBoardTaskRepository;
            _productImageRepository = productImageRepository;
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
        public IEventDonationRepository EventDonationRepository => _eventDonationRepository;
        public IEventTicketRepository EventTicketRepository => _eventTicketRepository;
        public IEventBoardRepository EventBoardRepository => _eventBoardRepository;
        public IEventBoardLabelRepository EventBoardLabelRepository => _eventBoardLabelRepository;
        public IEventBoardTaskLabelRepository EventBoardTaskLabelRepository => _eventBoardTaskLabelRepository;
        public IEventBoardColumnRepository EventBoardColumnRepository => _eventBoardColumnRepository;
        public IAttendeeRepository AttendeeRepository => _attendeeRepository;
        public IEventBoardTaskRepository EventBoardTaskRepository => _eventBoardTaskRepository;
        public IProductImageRepository ProductImageRepository => _productImageRepository;

        public Task<int> SaveChangeAsync()
        {
            try
            {
                return _studentEventForumDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}