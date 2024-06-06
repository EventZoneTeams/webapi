using Repositories.Commons;
using Repositories.Models;
using Services.DTO.ResponseModels;
using Services.DTO.UserModels;

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
        Task<ResponseGenericModel<UserDetailsModel>> UpdateStudentProfileAsync(int userId, UserUpdateModel userUpdateMode);
        Task<ResponseGenericModel<UserDetailsModel>> UpdateAccountAsync(int userId, UserUpdateModel userUpdateMode, string role);
        Task<ResponseGenericModel<UserDetailsModel>> CreateManagerAsync(UserSignupModel UserLogin);
        Task<ResponseGenericModel<List<UserDetailsModel>>> DeleteRangeUsers(List<int> userIds);
        Task<Pagination<UserDetailsModel>> GetUsersByFiltersAsync(PaginationParameter paginationParameter, UserFilterModel userFilterModel);
        Task<ResponseLoginModel> RefreshToken(TokenModel token);
    }
}
