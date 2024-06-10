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
        IEventFeedbackRepository EventFeedbackRepository { get; }

        Task<int> SaveChangeAsync();
    }
}