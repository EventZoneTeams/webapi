using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Commons;
using Repositories.DTO;
using Services.BusinessModels.EmailModels;
using Services.BusinessModels.UserModels;
using Services.Interface;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserSignupModel userLogin)
        {
            try
            {
                var data = await _userService.ResigerAsync(userLogin, "STUDENT");
                if (data.Status)
                {
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "users", new { email = userLogin.Email, token = data.Message }, Request.Scheme);
                    //var message = new Message(new string[] { data.Data.Email }, "Confirmation email link", confirmationLink!);
                    // await _emailService.SendEmail(message);
                    data.Message = "Added sucessfully <3";
                    return Ok(data);
                }

                return BadRequest(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            try
            {
                var result = await _userService.RefreshToken(model);
                if (result.Status.Equals(false))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] int id, [FromBody] UserUpdateModel userUpdatemodel, [FromQuery] RoleEnums? role)
        {
            try
            {
                var newRole = RoleEnums.ADMIN.Equals(role) ? "" : role.ToString();

                var result = await _userService.UpdateAccountAsync(id, userUpdatemodel, newRole);
                if (result.Status == false)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsersAsync([FromBody] List<int> userIds)
        {
            try
            {
                var result = await _userService.DeleteRangeUsers(userIds);
                if (result.Status)
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("manager")]
        public async Task<IActionResult> CreateUserAsync(UserSignupModel newUser)
        {
            try
            {
                var data = await _userService.CreateManagerAsync(newUser);
                if (data.Status)
                {
                    // var confirmationLink = Url.Action(nameof(ConfirmEmail), "users", new { email = userLogin.Email, token = data.Message }, Request.Scheme);
                    //var message = new Message(new string[] { data.Data.Email }, "Confirmation email link", confirmationLink!);
                    // await _emailService.SendEmail(message);
                    data.Message = "Created sucessfully <3";
                    return Ok(data);
                }

                return BadRequest(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginModel user)
        {
            try
            {
                var result = await _userService.LoginAsync(user);
                if (result.Status)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("wdqdwq")]
        //public async Task<IActionResult> GetAllusersAsync()
        //{
        //    return Ok(await _userService.GetAllUsers());
        //}

        [HttpGet()]
        public async Task<IActionResult> GetAccountByFilters([FromQuery] PaginationParameter paginationParameter, [FromQuery] UserFilterModel userFilterModel)
        {
            try
            {
                var result = await _userService.GetUsersByFiltersAsync(paginationParameter, userFilterModel);
                if (result == null)
                {
                    return NotFound("No accounts found with the specified filters.");
                }
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var result = await _userService.GetCurrentUserAsync();
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //[HttpGet("test-email")]
        //public async Task<IActionResult> TestEmail()
        //{
        //    var message = new Message(new string[]
        //    {
        //        "manhdung5289@gmail.com"
        //    },
        //     "Test",
        //     "<h1> Wassup bro ! </h1>"
        //    );

        //    await _emailService.SendEmail(message);

        //    return Ok(new { status = "success", Message = " email sent" });
        //}

        [HttpGet("confirm-email")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _userService.ConfirmEmail(email, token);
            if (result)
            {
                return Ok(new { status = true, message = "oh yeah lmao u did it" });
            }
            return BadRequest(new { status = false, message = "bruh we ded 💀" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            var result = await _userService.ForgotPassword(email);
            if (result.Status)
            {
                var confirmationLink = "Code:\n\"" + result.Data + "\"";
                var message = new Message(new string[] { email }, "Reset password token", confirmationLink!);
                await _emailService.SendEmail(message);
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            return Ok(await _userService.UserChangePasswordAsync(email, token, newPassword));
        }
    }
}