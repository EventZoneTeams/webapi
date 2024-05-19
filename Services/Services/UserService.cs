using AutoMapper;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.Interface;
using Services.ViewModels.ResponseModels;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<UserDetailsModel>> GetAllUsers()
        {
            var Users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            var result = new List<UserDetailsModel>();
            foreach (var User in Users)
            {
                var roleName = await _unitOfWork.UserRepository.GetRoleName(User);
                var lmao = _mapper.Map<UserDetailsModel>(User);
                //lmao.RoleName = roleName;
                result.Add(lmao);
            }

            //var result = await _unitOfWork.UserRepository.GetAllUsersWithRoleAsync();

            return result;
        }

        public Task<UserDetailsModel> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseGenericModel<UserDetailsModel>> ResigerAsync(UserSignupModel UserLogin, string role)
        {
            var result = await _unitOfWork.UserRepository.AddUser(UserLogin, role);
            if (result == null)
            {
                return new ResponseGenericModel<UserDetailsModel>
                {
                    Data = null,
                    Status = false,
                    Message = "Add User has been failed"
                };
            }

            var token = await _unitOfWork.UserRepository.GenerateEmailConfirmationToken(result);

            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = _mapper.Map<UserDetailsModel>(result),
                Status = true,
                Message = token
            };
        }

        public async Task<ResponseLoginModel> LoginAsync(UserLoginModel User)
        {
            return await _unitOfWork.UserRepository.LoginByEmailAndPassword(User);
        }

        public async Task<ResponseGenericModel<UserDetailsModel>> UserChangePasswordAsync(string email, string token, string newPassword)
        {
            var result = await _unitOfWork.UserRepository.ChangeUserPasswordAsync(email, token, newPassword);
            if (result == null)
            {
                return new ResponseGenericModel<UserDetailsModel> { Data = null, Status = false, Message = "The update process has been cooked, pleas try again" };
            }

            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = _mapper.Map<UserDetailsModel>(result),
                Status = true,
                Message = "Update Sucessfully"
            };

        }

        public async Task<ResponseGenericModel<string>> ForgotPassword(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new ResponseGenericModel<string> { Data = null, Status = false, Message = "User is not existed" };

            }
            else
            {
                return new ResponseGenericModel<string>
                {
                    Data = await _unitOfWork.UserRepository.GenerateTokenForResetPassword(user),
                    Status = true,
                    Message = "Token to your email " + user.Email + " have been sent for reset password"
                };
            }

        }
        public async Task<bool> ConfirmEmail(string email, string token)
        {
            return await _unitOfWork.UserRepository.ConfirmEmail(email, token);
        }




    }
}
