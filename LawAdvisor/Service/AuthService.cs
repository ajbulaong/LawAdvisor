using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LawAdvisor.Service
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		public async Task<ServiceResponseDto> CreateRole()
		{
			bool roleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

			if (roleExist)
			{
				return new ServiceResponseDto { IsSuccess = true, Message = "Role already exists" };
			}

			await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

			return new ServiceResponseDto { IsSuccess = true, Message = "Role successfully created" };
		}

		public async Task<ServiceResponseDto> Register([FromBody] UserDto request)
		{
			var isExist = await _userManager.FindByNameAsync(request.Username);

			if (isExist != null)
			{
				return new ServiceResponseDto { IsSuccess = false, Message = "User already exist" };
			}

			IdentityUser user = new IdentityUser()
			{
				UserName = request.Username,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var res = await _userManager.CreateAsync(user, request.Password);

			if (!res.Succeeded)
			{
				var errorString = "Encountered the following error(s): ";
				foreach (var error in res.Errors)
				{
					errorString += " | " + error.Description;
				}

				return new ServiceResponseDto { IsSuccess = false, Message = errorString };
			}

			await _userManager.AddToRoleAsync(user, StaticUserRoles.USER);

			return new ServiceResponseDto { IsSuccess = true, Message = "User successfully registered!" };
		}

		public async Task<ServiceResponseDto> Login([FromBody] UserDto request)
		{
			var user = await _userManager.FindByNameAsync(request.Username);

			if (user == null)
			{
				return new ServiceResponseDto { IsSuccess = false, Message = "User does not exist" };
			}

			var isCredentialCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

			if (!isCredentialCorrect)
			{
				return new ServiceResponseDto { IsSuccess = false, Message = "Invalid credentials" };
			}

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim("JwtId", Guid.NewGuid().ToString()),
			};

			foreach (var role in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var token = GenerateToken(authClaims);

			return new ServiceResponseDto { IsSuccess = true, Message = token };
		}

		private string GenerateToken(List<Claim> authClaims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var tokenObject = new JwtSecurityToken(
					issuer: _configuration["JWT:ValidIssuer"],
					audience: _configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(1),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
				);

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}
	}
}
