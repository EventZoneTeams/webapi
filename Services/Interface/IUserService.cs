using Domain.DTOs.EventOrderDTOs;
using Domain.DTOs.UserDTOs;
using Repositories.Commons;
using Repositories.Models;
using Services.DTO.UserModels;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<ApiResult<UserDetailsModel>> ResigerAsync(UserSignupModel UserLogin, string role);

        Task<UserDetailsModel> GetUserByEmail(string email);

        Task<ResponseLoginModel> LoginAsync(UserLoginModel User);

        Task<List<UserDetailsModel>> GetAllUsers();

        Task<ApiResult<UserDetailsModel>> UserChangePasswordAsync(string email, string currentPassword, string newPassword);

        Task<bool> ConfirmEmail(string email, string token);

        Task<ApiResult<string>> ForgotPassword(string email);

        Task<ApiResult<UserDetailsModel>> GetCurrentUserAsync();

        Task<ApiResult<UserDetailsModel>> UpdateStudentProfileAsync(Guid userId, UserUpdateModel userUpdateMode);

        Task<ApiResult<UserDetailsModel>> UpdateAccountAsync(Guid userId, UserUpdateModel userUpdateMode, string role);

        Task<ApiResult<UserDetailsModel>> CreateManagerAsync(UserSignupModel UserLogin);

        Task<ApiResult<List<UserDetailsModel>>> DeleteRangeUsers(List<Guid> userIds);

        Task<Pagination<UserDetailsModel>> GetUsersByFiltersAsync(PaginationParameter paginationParameter, UserFilterModel userFilterModel);

        Task<ResponseLoginModel> RefreshToken(TokenModel token);

        Task<UserDetailsModel> GetUserById(Guid id);

        Task<List<EventOrderReponseDTO>> GetCurrentUserOrders();

        Task<ApiResult<UserDetailsModel>> DeleteUser(Guid id);
    }
}