using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> AddAccount(AccountSignupModel account, String role);
        Task<User> ChangeAccountPasswordAsync(string id, string currentPassword, string newPassword);
        Task<bool> ConfirmEmail(string email, string token);
        Task<string> GenerateEmailConfirmationToken(User user);
        Task<string> GenerateTokenForResetPassword(User user);
        Task<User> GetAccountByEmailAsync(string email);
        Task<List<User>> GetAllAccountsAsync();
        Task<List<AccountDetailsModel>> GetAllAccountsWithRoleAsync();
        Task<List<string>> GetRoleName(User account);
        Task<ResponseLoginModel> LoginByEmailAndPassword(AccountLoginModel account);
    }
}
