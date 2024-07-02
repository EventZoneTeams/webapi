namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IEventRepository EventRepository { get; }
        IEventCategoryRepository EventCategoryRepository { get; }
        IEventProductRepository EventProductRepository { get; }
        IEventPackageRepository EventPackageRepository { get; }
        IWalletRepository WalletRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IEventFeedbackRepository EventFeedbackRepository { get; }
        IEventOrderRepository EventOrderRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IEventCampaignRepository EventCampaignRepository { get; }

        Task<int> SaveChangeAsync();
    }
}