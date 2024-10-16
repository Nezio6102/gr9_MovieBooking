using GR9_MovieBooking.Contexts;
using GR9_MovieBooking.Helpers;
using GR9_MovieBooking.Helpers.EmailSenderHelper;
using GR9_MovieBooking.Models;
using GR9_MovieBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web;

namespace GR9_MovieBooking.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly GR9MovieDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IPasswordHasher<IdentityUser> _passwordHasher;
		private readonly IEmailService _emailService;
		public AdminController(GR9MovieDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IPasswordHasher<IdentityUser> passwordHasher, IEmailService emailService)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_passwordHasher = passwordHasher;
			_emailService = emailService;
		}
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login(AdminLoginViewModel adminLoginViewModel)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(adminLoginViewModel.Username, adminLoginViewModel.Password, adminLoginViewModel.RememberMe, true);
				if (result.Succeeded)
				{
					return Ok();
				}
				if (result.IsLockedOut)
				{
					return Forbid();
				}
				else
				{
					return Unauthorized();
				}
			}
			return Ok(adminLoginViewModel);
		}
		[AllowAnonymous]
		[HttpPost("Forgot")]
		public async Task<IActionResult> Forgot(AdminForgotViewModel adminForgotViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(adminForgotViewModel.Email);
				if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
				{
					return BadRequest();
				}
				string token = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
				var resetLink = Url.Action("Reset", "Admin", new { adminForgotViewModel.Email, token }, Request.Scheme);
				EmailModel emailModel = new()
				{
					To = new List<string> { adminForgotViewModel.Email },
					Subject = "Password Reset",
					Content = System.IO.File.ReadAllText("Helpers/EmailHelper/EmailTemplate.html").Replace("{{username}}", user.UserName).Replace("{{action_url}}", resetLink),
				};
				await _emailService.Send(emailModel);
				ModelState.Clear();
				return Ok();
			}
			return BadRequest();
		}
		
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> Reset(AdminResetViewModel adminResetViewModel)
		{
			if (ModelState.IsValid)
			{
                string returnMessage = "";
                var user = await _userManager.FindByEmailAsync(adminResetViewModel.Email);
				if (user != null)
				{
					var token = HttpUtility.UrlDecode(adminResetViewModel.Token);
					var result = await _userManager.ResetPasswordAsync(user, token, adminResetViewModel.NewPassword);
					if (result.Succeeded)
					{
						
						if (await _userManager.IsLockedOutAsync(user))
						{
							await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
						}
						returnMessage = "Your password is reset. Please click <a href='/Admin/' class='alert-link'>here to login</a>";
					}
					else
					{
						returnMessage = result.Errors.ToString();
					}
					return Ok(returnMessage);
				}
			}
			return BadRequest(adminResetViewModel);
		}
		
		[HttpPost("ChangePassword")]
		public async Task<IActionResult> Account(AdminAccountViewModel adminAccountViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = _context.Users.Find(_userManager.GetUserId(User));
				if (user != null)
				{
					var result = await _userManager.ChangePasswordAsync(user, adminAccountViewModel.CurrentPassword, adminAccountViewModel.NewPassword);
					if (result.Succeeded)
					{
						await _signInManager.RefreshSignInAsync(user);
						return Ok();
					}
					else
					{
						return BadRequest();
					}
				}
				else
				{
					return NotFound();
				}
			}
			return Ok();
		}
		[HttpPost("isAuthenticated")]
		public async Task<IActionResult> isAuthenticated(string userId)
		{
			if(userId == null)
			{
				return Unauthorized();
			}
			if(User.Identity.IsAuthenticated && User.FindFirst(ClaimTypes.NameIdentifier)?.Value== userId)
			{
				return Ok();
			}
			return Unauthorized();
		}
		

	}
}