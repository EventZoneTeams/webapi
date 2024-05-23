using Repositories.DTO;
using Services.ViewModels.ResponseModels;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<ResponseGenericModel<UserDetailsModel>> ResigerAsync(UserSignupModel UserLogin, string role);
        Task<UserDetailsModel> GetUserByEmail(string email);
        Task<ResponseLoginModel> LoginAsync(UserLoginModel User);
        Task<List<UserDetailsModel>> GetAllUsers();
        Task<ResponseGenericModel<UserDetailsModel>> UserChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<bool> ConfirmEmail(string email, string token);
        Task<ResponseGenericModel<string>> ForgotPassword(string email);
        Task<ResponseGenericModel<UserDetailsModel>> GetCurrentUserAsync();
    }
}
