using Domain.DTOs.UserDTOs;
using Domain.Entities;
using Repositories.Commons;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUser(UserSignupModel user, String role);

        Task<User> ChangeUserPasswordAsync(string email, string currentPassword, string newPassword);

        Task<bool> ConfirmEmail(string email, string token);

        Task<string> GenerateEmailConfirmationToken(User user);

        Task<string> GenerateTokenForResetPassword(User user);

        Task<User> GetUserByEmailAsync(string email);

        Task<List<User>> GetAllUsersAsync();

        Task<List<UserDetailsModel>> GetAllUsersWithRoleAsync();

        Task<List<string>> GetRoleName(User user);

        Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel user);

        Task<User> GetCurrentUserAsync();

        Task<User> GetAccountDetailsAsync(Guid userId);

        Task<User> UpdateAccountAsync(User user);

        Task<string> UpdateUserRole(User user, string role);

        Task<List<User>> SoftRemoveRangeUserAsync(List<Guid> userIds);

        Task<Pagination<User>> GetUsersByFiltersAsync(PaginationParameter paginationParameter, UserFilterModel UserFilterModel);

        Task<List<string>> GetAllRoleNamesAsync();

        Task<ResponseLoginModel> RefreshToken(TokenModel token);

        Task<User> GetUserByIdAsync(Guid id);

        Task<User> SoftRemoveUserAsync(Guid id);
    }
}