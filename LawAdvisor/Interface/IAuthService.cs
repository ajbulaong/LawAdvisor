using LawAdvisor.Models;
using Microsoft.AspNetCore.Mvc;

namespace LawAdvisor.Interface
{
	public interface IAuthService
	{
		Task<ServiceResponseDto> CreateRole();
		Task<ServiceResponseDto> Register([FromBody] UserDto request);
		Task<ServiceResponseDto> Login([FromBody] UserDto request);

	}
}
