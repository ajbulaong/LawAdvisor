using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LawAdvisor.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		
		[HttpPost]
		[Route("role/create")]
		public async Task<IActionResult> CreateRole()
		{
			var res = await _authService.CreateRole();
			return Ok(res);
		}

		[HttpPost]
		[Route("user/register")]
		public async Task<IActionResult> Register([FromBody] UserDto request)
		{
			var res = await _authService.Register(request);
			if(res.IsSuccess)
			{
				return Ok(res);
			}
			
			return BadRequest(res);
		}

		[HttpPost]
		[Route("user/login")]
		public async Task<IActionResult> Login([FromBody] UserDto request)
		{
			var res = await _authService.Login(request);
			if (res.IsSuccess)
			{
				return Ok(res);
			}

			return Unauthorized(res);
		}

	}
}
