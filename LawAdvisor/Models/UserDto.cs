using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LawAdvisor.Models
{
	public class UserDto
	{
		[Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }
		[Required(ErrorMessage = "Password is required!")]
		public string Password { get; set; }
    }
}
