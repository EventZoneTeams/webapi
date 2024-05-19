namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        Task<int> SaveChangeAsync();
    }
}
