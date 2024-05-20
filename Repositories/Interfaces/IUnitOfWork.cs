namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IEventRepository EventRepository { get; }

        Task<int> SaveChangeAsync();
    }
}
