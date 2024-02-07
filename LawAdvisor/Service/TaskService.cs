using Azure.Core;
using LawAdvisor.Data;
using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace LawAdvisor.Service
{
	public class TaskService : ITaskService
	{
		private readonly ApplicationDbContext _context;
		public TaskService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<ServiceResponseDto> CreateTask(Tasks request)
		{
			try
			{
				_context.Tasks.Add(request);
				await _context.SaveChangesAsync();

				return new ServiceResponseDto
				{
					IsSuccess = true,
					Message = "Task was successfully created!"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponseDto
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
			
		}

		public async Task<Tasks> GetTaskById(int taskId)
		{
			var res = await _context.Tasks.FindAsync(taskId);
			return res;
		}

		public async Task<List<Tasks>> GetTasks()
		{
			var res = await _context.Tasks.ToListAsync();
			return res;
		}

		public async Task<ServiceResponseDto> UpdateTask(Tasks request)
		{
			try
			{
				_context.Entry(request).State = EntityState.Modified;

				await _context.SaveChangesAsync();

				return new ServiceResponseDto
				{
					IsSuccess = true,
					Message = "Task was successfully updated"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponseDto
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
			
		}
	}
}
