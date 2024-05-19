using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Commons;
using Repositories.DTO;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentEventForumDbContext _templateDbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;
        private readonly IConfiguration _configuration;
        // identity collection
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(StudentEventForumDbContext templateDbContext, ICurrentTime timeService, IClaimsService claimsService, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _templateDbContext = templateDbContext;
            _timeService = timeService;
            _claimsService = claimsService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<List<string>> GetRoleName(User User)
        {
            var result = await _userManager.GetRolesAsync(User);
            return result.ToList();
        }

        public async Task<User> AddUser(UserSignupModel User, string role)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(User.Email);
                if (userExist != null)
                {
                    return null;
                }

                var user = new User
                {
                    Email = User.Email,
                    UserName = User.Email,
                    FullName = User.FullName,
                    Dob = User.Dob,
                    Gender = User.Gender.ToLower() == "male" ? true : false,
                    CreatedBy = _claimsService.GetCurrentUserId,
                    CreatedDate = _timeService.GetCurrentTime()
                };

                if (User.FullName != null)
                {
                    user.UnsignFullName = StringTools.ConvertToUnSign(User.FullName);
                }

                var result = await _userManager.CreateAsync(user, User.Password);

                if (result.Succeeded)
                {
                    Console.WriteLine($"New user ID: {user.Id}");
                    if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    {
                        //await _roleManager.CreateAsync(new Role(role.ToString()));
                    }

                    if (await _roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await _userManager.AddToRoleAsync(user, role.ToString());
                    }

                    if (!await _roleManager.RoleExistsAsync(role.ToString()))
                        //await _roleManager.CreateAsync(new IdentityRole(role));

                        if (await _roleManager.RoleExistsAsync(role.ToString()))
                        {
                            await _userManager.AddToRoleAsync(user, role.ToString());
                        }
                    return user;
                }
                else
                {
                    // Tạo người dùng không thành công, xem thông tin lỗi và xử lý
                    StringBuilder errorValue = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        errorValue.Append($"{item.Description}");
                    }
                    throw new Exception(errorValue.ToString()); // bắn zề cho GlobalEx midw

                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GenerateTokenForResetPassword(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel User)
        {
            var UserExist = await _userManager.FindByEmailAsync(User.Email);
            if (UserExist == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(User.Email, User.Password, false, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(UserExist);

                var authClaims = new List<Claim> // add User vào claim
                {
                    //new Claim("UserId", 1),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                //generate refresh token
                var refreshToken = TokenTools.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                UserExist.RefreshToken = refreshToken;
                UserExist.RefreshTokenExpiryTime = _timeService.GetCurrentTime().AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(UserExist); //update 2 jwt
                var token = GenerateJWTToken.CreateToken(authClaims, _configuration, _timeService.GetCurrentTime());
                return new ResponseLoginModel
                {
                    Status = true,
                    Message = "Login successfully",
                    JWT = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo,
                    JWTRefreshToken = refreshToken,
                };
            }
            else
            {
                if (!UserExist.EmailConfirmed)
                {
                    return new ResponseLoginModel
                    {
                        Status = false,
                        Message = "Your email haven't verified yet, please check",
                    };
                }

                return new ResponseLoginModel
                {
                    Status = false,
                    Message = "Incorrect email or password",
                };
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                return null;
            }
            return result;

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                // get all users
                var Users = await _userManager.Users.ToListAsync();
                return Users;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<UserDetailsModel>> GetAllUsersWithRoleAsync()
        {
            // Bước 1: Lấy danh sách người dùng
            var users = _userManager.Users;

            // Bước 2: Kết hợp thông tin người dùng, vai trò và lấy danh sách UserDetailsModel ban đầu
            var UserDetailsModels = await (from user in users
                                           join userRole in _templateDbContext.UserRoles on user.Id equals userRole.UserId
                                           join role in _templateDbContext.Roles on userRole.RoleId equals role.Id
                                           group new { user, role } by user.Id into userRolesGroup
                                           select new UserDetailsModel
                                           {
                                               Id = userRolesGroup.Key,
                                               UnsignFullName = userRolesGroup.First().user.UnsignFullName,
                                               FullName = userRolesGroup.First().user.FullName,
                                               Dob = userRolesGroup.First().user.Dob,
                                               Gender = (bool)userRolesGroup.First().user.Gender ? "male" : "female",
                                               Image = userRolesGroup.First().user.Image,
                                               IsDeleted = userRolesGroup.First().user.IsDeleted,
                                               Role = userRolesGroup.Select(urg => new RoleInfoModel
                                               {
                                                   RoleId = urg.role.Id,
                                                   RoleName = urg.role.Name
                                               }).ToList()
                                           }).ToListAsync();

            return UserDetailsModels;
            return null;
        }

        public async Task<User> ChangeUserPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var User = await _userManager.FindByEmailAsync(email);
                if (User == null)
                {
                    return null;
                }

                var changePasswordResult = await _userManager.ResetPasswordAsync(User, token, newPassword);
                if (changePasswordResult.Succeeded)
                {
                    _templateDbContext.Update(User);
                    return User;
                }
                else
                {
                    var errorMessage = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
                    throw new Exception($"Error changing password: {errorMessage}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _templateDbContext.Update(user);
                    await _templateDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    // Verify người dùng không thành công, xem thông tin lỗi và xử lý
                    StringBuilder errorValue = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        errorValue.Append($"{item.Description}");
                    }
                    throw new Exception(errorValue.ToString()); // bắn zề cho GlobalEx midw
                }
            }
            return false;
        }

        public async Task<Pagination<User>> GetUsersByFiltersAsync(PaginationParameter paginationParameter, UserFilterModel UserFilterModel)
        {
            var UsersQuery = _templateDbContext.Users.AsQueryable();
            UsersQuery = await ApplyFilterSortAndSearch(UsersQuery, UserFilterModel);
            if (UsersQuery != null)
            {
                var sortedQuery = ApplySorting(UsersQuery, UserFilterModel);
                var totalCount = await sortedQuery.CountAsync();
                var UsersPagination = await sortedQuery
                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                    .Take(paginationParameter.PageSize)
                    .ToListAsync();
                return new Pagination<User>(UsersPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            }
            return null;
        }



        private IQueryable<User> ApplySorting(IQueryable<User> query, UserFilterModel UserFilterModel)
        {
            switch (UserFilterModel.Sort.ToLower())
            {
                case "fullname":
                    query = (UserFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.FullName) : query.OrderByDescending(a => a.FullName);
                    break;
                case "dob":
                    query = (UserFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Dob) : query.OrderByDescending(a => a.Dob);
                    break;
                default:
                    query = (UserFilterModel.SortDirection.ToLower() == "asc") ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
                    break;
            }
            return query;
        }

        private async Task<IQueryable<User>> ApplyFilterSortAndSearch(IQueryable<User> query, UserFilterModel UserFilterModel)
        {
            if (UserFilterModel == null)
            {
                return query;
            }

            if (UserFilterModel.isDeleted == true)
            {
                query = query.Where(a => a.IsDeleted == true);
            }
            else if (UserFilterModel.isDeleted == false)
            {
                query = query.Where(a => a.IsDeleted == false);
            }
            else
            {
                query = query.Where(a => a.IsDeleted == true || a.IsDeleted == false);
            }

            if (!string.IsNullOrEmpty(UserFilterModel.Gender))
            {
                bool isMale = UserFilterModel.Gender.ToLower() == "male";
                query = query.Where(a => a.Gender == isMale);
            }

            if (!string.IsNullOrEmpty(UserFilterModel.Role))
            {
                var UsersInRole = await _userManager.GetUsersInRoleAsync(UserFilterModel.Role);

                if (UsersInRole != null)
                {
                    var userIdsInRole = UsersInRole.Select(u => u.Id);
                    query = query.Where(a => userIdsInRole.Contains(a.Id));
                }
                else
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(UserFilterModel.Search))
            {
                query = query.Where(a =>
                    a.FullName.Contains(UserFilterModel.Search) ||
                    a.UnsignFullName.Contains(UserFilterModel.Search)
                );
            }
            return query;



        }

    }
}
