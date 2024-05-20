﻿using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.Interface;
using Services.ViewModels.EmailModels;

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
        public async Task<IActionResult> RegisterAsync(UserSignupModel userLogin, [FromQuery] RoleEnums role)
        {
            try
            {
                var data = await _userService.ResigerAsync(userLogin, role.ToString());
                if (data.Status)
                {
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "users", new { email = userLogin.Email, token = data.Message }, Request.Scheme);
                    var message = new Message(new string[] { data.Data.Email }, "Confirmation email link", confirmationLink!);
                    await _emailService.SendEmail(message);
                    data.Message = "Added sucessfully, please check your email <3";
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
                if (result != null)
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

        [HttpGet]
        public async Task<IActionResult> GetAllusersAsync()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new Message(new string[]
            {
                "manhdung5289@gmail.com"
            },
             "Test",
             "<h1> Wassup bro ! </h1>"
            );

            await _emailService.SendEmail(message);

            return Ok(new { status = "success", Message = " email sent" });
        }

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

        [HttpGet("forget-passwrord")]
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

        [HttpPut("password-reset")]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            return Ok(await _userService.UserChangePasswordAsync(email, token, newPassword));
        }






    }
}