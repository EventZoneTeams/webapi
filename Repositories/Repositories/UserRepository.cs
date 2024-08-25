using EventZone.Domain.DTOs.UserDTOs;
using EventZone.Domain.Entities;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Models.UserAuthenModels;
using EventZone.Repositories.Utils;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventZone.Repositories.Repositories
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

        public UserRepository(StudentEventForumDbContext templateDbContext, ICurrentTime timeService,
            IClaimsService claimsService, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IConfiguration configuration)
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
            if (result != null && result.Count > 0)
            {
                return result.ToList();
            }
            result.Add("");
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
                    ImageUrl = User.ImageUrl,
                    WorkAt = User.WorkAt,
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
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        var newRole = new Role();
                        newRole.Name = role;
                        await _roleManager.CreateAsync(newRole);
                    }

                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    if (!await _roleManager.RoleExistsAsync(role))
                        //await _roleManager.CreateAsync(new IdentityRole(role));

                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
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
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GenerateTokenForResetPassword(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == _claimsService.GetCurrentUserId);
            if (user != null)
            {
                return user;
            }

            return null;
        }

        public async Task<ResponseLoginModel> LoginGoogleAsync(string credential)
        {
            string clientID = _configuration["GoogleCredential:ClientId"];
            string clientID1 = _configuration["GoogleCredential2:ClientId"];

            if (string.IsNullOrEmpty(clientID) && string.IsNullOrEmpty(clientID1))
            {
                throw new Exception("CliendId Is null!");
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
                {
                    clientID,clientID1
                }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
            if (payload == null)
            {
                throw new Exception("Credential incorrect!");
            }

            var accountExist = await _userManager.FindByEmailAsync(payload.Email);

            if (accountExist != null)
            {
                if (accountExist.IsDeleted == true)
                {
                    throw new Exception("Account is not existing or banned");
                }

                if (payload.Picture != null)
                {
                    if (accountExist.ImageUrl == null)
                    {
                        accountExist.ImageUrl = payload.Picture;
                        await _userManager.UpdateAsync(accountExist);
                    }
                    else
                    {
                        if (payload.Picture != accountExist.ImageUrl)
                        {
                            accountExist.ImageUrl = payload.Picture;
                            await _userManager.UpdateAsync(accountExist);
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(accountExist);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accountExist.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var refreshToken = TokenTools.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            accountExist.RefreshToken = refreshToken;
            accountExist.RefreshTokenExpiryTime = _timeService.GetCurrentTime().AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(accountExist);

            var token = GenerateJWTToken.CreateToken(authClaims, _configuration, DateTime.UtcNow);

            return new ResponseLoginModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expired = token.ValidTo,
                RefreshToken = refreshToken,
                UserId = accountExist.Id
            };
        }

        public async Task<ResponseLoginModel> RefreshToken(TokenModel token)
        {
            if (token is null)
            {
                throw new Exception("Token is null");
            }

            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var principal = TokenTools.GetPrincipalFromExpiredToken(accessToken, _configuration);
            if (principal == null)
            {
                throw new Exception("Invalid access token or refresh token!");
            }

            string accountId = principal.Identity.Name;

            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null || account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid access token or refresh token!");
            }

            var newAccessToken = GenerateJWTToken.CreateToken(principal.Claims.ToList(), _configuration, _timeService.GetCurrentTime());
            var newRefreshToken = TokenTools.GenerateRefreshToken();

            account.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(account);

            return new ResponseLoginModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Expired = newAccessToken.ValidTo,
                RefreshToken = newRefreshToken,
                UserId = Guid.Parse(accountId)
            };
        }

        public async Task<ResponseLoginModel> LoginByEmailAndPassword(UserLoginModel User)
        {
            var UserExist = await _userManager.FindByEmailAsync(User.Email);
            if (UserExist == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(UserExist, User.Password, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(UserExist);

                var authClaims = new List<Claim> // add User vào claim
                {
                    new Claim(ClaimTypes.Name, UserExist.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo,
                    RefreshToken = refreshToken,
                    UserId = UserExist.Id
                };
            }
            else
            {
                throw new Exception("Invalid login attempt. Please check your password.");
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

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var result = await _userManager.FindByIdAsync(id.ToString());
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<User> GetAccountDetailsAsync(Guid userId)
        {
            var accounts = await _userManager.FindByIdAsync(userId.ToString());
            var account = await _templateDbContext.Users.FirstOrDefaultAsync(a => a.Id == userId);
            if (account == null)
            {
                return null;
            }
            return account;
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

        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            try
            {
                // get all users
                var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
                return roles;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<User> UpdateAccountAsync(User user)
        {
            try
            {
                user.ModifiedDate = _timeService.GetCurrentTime();
                user.ModifiedBy = _claimsService.GetCurrentUserId;
                user.UnsignFullName = StringTools.ConvertToUnSign(user.FullName);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
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
        }

        public async Task<List<User>> SoftRemoveRangeUserAsync(List<Guid> userIds)
        {
            try
            {
                var users = await _userManager.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
                foreach (var user in users)
                {
                    user.IsDeleted = true;
                    user.DeletionDate = _timeService.GetCurrentTime();
                    user.DeleteBy = _claimsService.GetCurrentUserId;
                    _templateDbContext.Entry(user).State = EntityState.Modified;
                    // await _dbContext.SaveChangesAsync();
                }
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> SoftRemoveUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new Exception("This user is not existed");
                }

                user.IsDeleted = true;
                user.DeletionDate = _timeService.GetCurrentTime();
                user.DeleteBy = _claimsService.GetCurrentUserId;
                _templateDbContext.Entry(user).State = EntityState.Modified;
                // await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateUserRole(User user, string role)
        {
            try
            {
                // Lấy danh sách vai trò hiện tại của người dùng
                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Count == 0)
                {
                    return null;
                }

                if (role.ToLower() == currentRoles.First().ToLower())
                {
                    return currentRoles.First();
                }

                // Xóa tất cả vai trò hiện tại của người dùng
                var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (result.Succeeded)
                {
                    // Kiểm tra xem vai trò mới có tồn tại hay không
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        var newRole = new Role();
                        newRole.Name = role;
                        // Vai trò mới không tồn tại, tạo mới vai trò
                        await _roleManager.CreateAsync(newRole);
                    }

                    result = await _userManager.AddToRoleAsync(user, role);

                    if (result.Succeeded)
                    {
                        return role;
                    }
                }

                // Tạo người dùng không thành công, xem thông tin lỗi và xử lý
                StringBuilder errorValue = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    errorValue.Append($"{item.Description}");
                }
                throw new Exception(errorValue.ToString()); // bắn zề cho GlobalEx midw
            }
            catch (Exception)
            {
                throw;
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
                                               Email = userRolesGroup.First().user.Email,
                                               Dob = userRolesGroup.First().user.Dob,
                                               Gender = (bool)userRolesGroup.First().user.Gender ? "Male" : "Female",
                                               ImageUrl = userRolesGroup.First().user.ImageUrl,
                                               IsDeleted = userRolesGroup.First().user.IsDeleted,
                                               //Role = userRolesGroup.Select(urg => new RoleInfoModel
                                               //{
                                               //    RoleId = urg.role.Id,
                                               //    RoleName = urg.role.Name
                                               //}).ToList()
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
                throw new Exception();
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
            var UsersQuery = _templateDbContext.Users.AsNoTracking();
            UsersQuery = await ApplyFilterSortAndSearch(UsersQuery, UserFilterModel);
            if (UsersQuery != null)
            {
                var sortedQuery = await ApplySorting(UsersQuery, UserFilterModel).ToListAsync();
                var totalCount = sortedQuery.Count;
                var UsersPagination = sortedQuery
                    .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                    .Take(paginationParameter.PageSize)
                    .ToList();
                return new Pagination<User>(UsersPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
            }
            return null;
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, UserFilterModel UserFilterModel)
        {
            switch (UserFilterModel.SortBy.ToLower())
            {
                case "fullname":
                    query = UserFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.FullName) : query.OrderByDescending(a => a.FullName);
                    break;

                case "dob":
                    query = UserFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Dob) : query.OrderByDescending(a => a.Dob);
                    break;

                default:
                    query = UserFilterModel.SortDirection.ToLower() == "asc" ? query.OrderBy(a => a.Id) : query.OrderByDescending(a => a.Id);
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
                query = query.Where(a => a.IsDeleted == UserFilterModel.isDeleted);
            }
            else if (UserFilterModel.isDeleted == false)
            {
                query = query.Where(a => a.IsDeleted == UserFilterModel.isDeleted);
            }

            if (!string.IsNullOrEmpty(UserFilterModel.Gender))
            {
                bool isMale = UserFilterModel.Gender.ToLower() == "male";
                query = query.Where(a => a.Gender == isMale);
            }

            if (!string.IsNullOrEmpty(UserFilterModel.Role))
            {
                var UsersInRole = await _userManager.GetUsersInRoleAsync(UserFilterModel.Role.ToUpper());

                if (UsersInRole != null && UsersInRole.Count > 0)
                {
                    var userIdsInRole = UsersInRole.Select(u => u.Id);
                    query = query.Where(a => userIdsInRole.Contains(a.Id));
                }
                else
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(UserFilterModel.SearchName))
            {
                query = query.Where(a =>
                    a.FullName.Contains(UserFilterModel.SearchName) ||
                    a.UnsignFullName.Contains(UserFilterModel.SearchName)
                );
            }

            return query;
        }
    }
}