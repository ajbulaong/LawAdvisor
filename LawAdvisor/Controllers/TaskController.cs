using LawAdvisor.Data;
using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawAdvisor.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;
		public TaskController(ITaskService taskService)
		{
			_taskService = taskService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTasks() 
		{
			var res = await _taskService.GetTasks();
			if(res == null)
			{
				return NoContent(); //Empty or no record
			}

			return Ok(res);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetTask(int id)
		{
			var res = await _taskService.GetTaskById(id);
			if (res == null)
			{
				return NoContent(); //Empty or no record
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTask([FromBody] Tasks request)
		{
			var res = await _taskService.CreateTask(request);
            if (!res.IsSuccess)
            {
				return StatusCode(500, res);
            }

            return CreatedAtAction(nameof(GetTask), new { id = request.Id }, request);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTask(int id, Tasks request)
		{
			if(id != request.Id)
			{
				return BadRequest();
			}
			
			var res = await _taskService.UpdateTask(request);
			if (!res.IsSuccess)
			{
				return StatusCode(500, res);
			}
			return Ok();
		}
	}
}
