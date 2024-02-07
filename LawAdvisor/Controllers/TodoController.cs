using Azure.Core;
using LawAdvisor.Data;
using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LawAdvisor.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly IToDoService _toDo;
		public TodoController(IToDoService toDo)
		{
			_toDo = toDo;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetToDoList(string id)
		{
			var res = await _toDo.GetToDoList(id);
			if(res == null)
			{
				return NotFound();
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> AssignTask(int task, string userId)
		{
			var toDoItem = new ToDo
			{
				UserId = userId,
				TaskId = task
			};

			var res = await _toDo.AssignTask(toDoItem);
			if(!res.IsSuccess)
			{
				return BadRequest(res);
			}

			return Ok(res);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Rearrange(int id, int newPos)
		{
			await _toDo.RearrangeTask(id, newPos);
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> RemoveTask(int id)
		{
			var res = await _toDo.RemoveTask(id);
			if(!res.IsSuccess)
			{
				if(res.Message == "ToDo item cannot be found")
				{
					return BadRequest(res);
				}
				else
				{
					return StatusCode(500, res);
				}
			}

			return NoContent();
		}
	}
}
