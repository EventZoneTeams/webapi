using AutoMapper;
using Repositories.Commons;
using Repositories.DTO;
using Repositories.Interfaces;
using Services.BusinessModels.EmailModels;
using Services.BusinessModels.EventProductsModel;
using Services.BusinessModels.ResponseModels;
using Services.BusinessModels.UserModels;
using Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //var Users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            //var result = new List<UserDetailsModel>();
            //foreach (var User in Users)
            //{
            //    var roleName = await _unitOfWork.UserRepository.GetRoleName(User);
            //    var lmao = _mapper.Map<UserDetailsModel>(User);
            //    lmao.RoleName = roleName;
            //    result.Add(lmao); 
            //}

            var result = await _unitOfWork.UserRepository.GetAllUsersWithRoleAsync();

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
                    Message = "User have been existed"
                };
            }

            //  var token = await _unitOfWork.UserRepository.GenerateEmailConfirmationToken(result);

            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = _mapper.Map<UserDetailsModel>(result),
                Status = true,
                Message = "Register Successfuly"
            };
        }

        public async Task<ResponseLoginModel> RefreshToken(TokenModel token)
        {
            return await _unitOfWork.UserRepository.RefreshToken(token);
        }

        public async Task<ResponseGenericModel<UserDetailsModel>> UpdateStudentProfileAsync(int userId, UserUpdateModel userUpdateMode)
        {
            var existingUser = await _unitOfWork.UserRepository.GetAccountDetailsAsync(userId);
            if (existingUser != null)
            {
                existingUser = _mapper.Map(userUpdateMode, existingUser);
                var updatedAccount = await _unitOfWork.UserRepository.UpdateAccountAsync(existingUser);

                if (updatedAccount != null)
                {
                    var response = new ResponseGenericModel<UserDetailsModel>();
                    response.Data = _mapper.Map<UserDetailsModel>(existingUser);
                    response.Message = "Updated user successfully";
                    response.Status = true;
                    return response;
                }

            }

            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = null,
                Status = false,
                Message = "This user is not existed"
            };

        }

        public async Task<ResponseGenericModel<UserDetailsModel>> UpdateAccountAsync(int userId, UserUpdateModel userUpdateMode, string role)
        {
            try
            {
                var existingUser = await _unitOfWork.UserRepository.GetAccountDetailsAsync(userId);
                if (existingUser != null)
                {
                    existingUser = _mapper.Map(userUpdateMode, existingUser);
                    // exiistingUser.RoleId = EnumHelper.ConvertToRoleId(accountUpdateModel.Role);
                    var updatedAccount = await _unitOfWork.UserRepository.UpdateAccountAsync(existingUser);

                    if (updatedAccount != null)
                    {
                        var response = new ResponseGenericModel<UserDetailsModel>();
                        response.Data = _mapper.Map<UserDetailsModel>(existingUser);
                        response.Message = "Updated user successfully";
                        response.Status = true;
                        if (!string.IsNullOrEmpty(role))
                        {
                            var result = await _unitOfWork.UserRepository.UpdateUserRole(existingUser, role);
                            if (result.Equals(role))
                            {
                                response.Data.RoleName = role;
                                response.Message = "Updated user and role Successfuly";
                            }
                            else
                            {

                                response.Message = "Updated user successfully but role are not changed";
                            }
                        }
                        return response;
                    }

                }

                return new ResponseGenericModel<UserDetailsModel>
                {
                    Data = null,
                    Status = false,
                    Message = "This user is not existed"
                };
            }
            catch (Exception)
            {

                throw;
            }

        }




        public async Task<ResponseGenericModel<UserDetailsModel>> CreateManagerAsync(UserSignupModel UserLogin)
        {
            var result = await _unitOfWork.UserRepository.AddUser(UserLogin, "MANAGER");
            if (result == null)
            {
                return new ResponseGenericModel<UserDetailsModel>
                {
                    Data = null,
                    Status = false,
                    Message = "User have been existed"
                };
            }

            //  var token = await _unitOfWork.UserRepository.GenerateEmailConfirmationToken(result);

            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = _mapper.Map<UserDetailsModel>(result),
                Status = true,
                Message = ""
            };
        }


        public async Task<ResponseGenericModel<UserDetailsModel>> GetCurrentUserAsync()
        {
            var result = await _unitOfWork.UserRepository.GetCurrentUserAsync();
            if (result != null)
            {
                var role = await _unitOfWork.UserRepository.GetRoleName(result);
                var data = _mapper.Map<UserDetailsModel>(result);
                data.RoleName = role.First();

                return new ResponseGenericModel<UserDetailsModel>
                {
                    Data = data,
                    Status = true,
                    Message = "This is current user"
                };
             
            }
            return new ResponseGenericModel<UserDetailsModel>
            {
                Data = null,
                Status = false,
                Message = "User is not found due to error or expiration token"
            };


        }

        public async Task<ResponseGenericModel<List<UserDetailsModel>>> DeleteRangeUsers(List<int> userIds)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            var existingIds = users.Where(e => userIds.Contains(e.Id)).Select(e => e.Id).ToList();
            var nonExistingIds = userIds.Except(existingIds).ToList();

            if (existingIds.Count > 0)
            {
                 await _unitOfWork.UserRepository.SoftRemoveRangeUserAsync(existingIds);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0 )
                {
                    return new ResponseGenericModel<List<UserDetailsModel>>()
                    {
                        Status = true,
                        Message = " Added successfully",
                        Data = _mapper.Map<List<UserDetailsModel>>(users.Where(e => existingIds.Contains(e.Id)))
                    };
                }
            }
            else
            {
                if (nonExistingIds.Count > 0)
                {
                    string nonExistingIdsString = string.Join(", ", nonExistingIds);

                    return new ResponseGenericModel<List<UserDetailsModel>>()
                    {
                        Status = false,
                        Message = "There are few ids that is not existed user: " + nonExistingIdsString,
                        Data = _mapper.Map<List<UserDetailsModel>>(users.Where(e => existingIds.Contains(e.Id)))
                    };
                }

            }
            return new ResponseGenericModel<List<UserDetailsModel>>()
            {
                Status = false,
                Message = "failed",
                Data = null
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

        public async Task<Pagination<UserDetailsModel>> GetUsersByFiltersAsync(PaginationParameter paginationParameter, UserFilterModel userFilterModel)
        {
            var accounts = await _unitOfWork.UserRepository.GetUsersByFiltersAsync(paginationParameter, userFilterModel);
            var mappedResult = new List<UserDetailsModel>();
            //var roleNames = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            if (accounts != null)
            {
                foreach (var model in accounts)
                {
                    var mappedModel = _mapper.Map<UserDetailsModel>(model);
                    mappedModel.Gender = model.Gender == true ? "Male" : "Female";
                    mappedModel.RoleName = (await _unitOfWork.UserRepository.GetRoleName(model)).First();
                    mappedResult.Add(mappedModel);
                }
                return new Pagination<UserDetailsModel>(mappedResult, accounts.TotalCount, accounts.CurrentPage, accounts.PageSize);
            }
            return null;
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
