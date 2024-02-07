using LawAdvisor.Data;
using LawAdvisor.Interface;
using LawAdvisor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace LawAdvisor.Service
{
	public class ToDoService : IToDoService
	{
		private readonly ApplicationDbContext _context;
		public ToDoService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<ServiceResponseDto> AssignTask(ToDo request)
		{
			try
			{
				var res = GetLastIntegerValueAsync(request.UserId);
				if(res < 0)
				{
					return new ServiceResponseDto
					{
						IsSuccess = false,
						Message = "Cannot get the order number"
					};
				}

				request.Order = res;

				_context.ToDos.Add(request);
				await _context.SaveChangesAsync();

				return new ServiceResponseDto
				{
					IsSuccess = true,
					Message = "The task was successfully added to ToDo List"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponseDto
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}

		}

		public async Task<List<ToDo>> GetToDoList(string userId)
		{
			var tasks = await _context.ToDos
					.Where(tdl => tdl.UserId == userId)
					.ToListAsync();

			return tasks;
		}

		public async Task<ServiceResponseDto> RearrangeTask(int id, int newPos)
		{
			int fromPosition = 0;
			int toPosition = newPos;
			//Get record
			var recordToMove = _context.ToDos.SingleOrDefault(e => e.Id == id);
			if (recordToMove == null)
			{	
				return new ServiceResponseDto
				{
					IsSuccess = false,
					Message = "Record cannot be found"
				};
			}

			fromPosition = recordToMove.Order;

			if (fromPosition < toPosition)
			{
				// Move up
				_context.ToDos.Where(e => e.Order > fromPosition && e.Order <= toPosition)
							.ToList()
							.ForEach(e => e.Order--);
			}
			else if (fromPosition > toPosition)
			{
				// Move down
				_context.ToDos.Where(e => e.Order >= toPosition && e.Order < fromPosition)
							.ToList()
							.ForEach(e => e.Order++);
			}

			recordToMove.Order = toPosition;
			await _context.SaveChangesAsync();

			return new ServiceResponseDto { IsSuccess = true, Message = "Records has been successfully rearranged"};
		}
		public async Task<ServiceResponseDto> RemoveTask(int id)
		{
			try
			{
				var toDoItem = await _context.ToDos
					.SingleOrDefaultAsync(tdl => tdl.Id == id);

				if (toDoItem == null)
				{
					return new ServiceResponseDto
					{
						IsSuccess = false,
						Message = "ToDo item cannot be found"
					};
				}

				_context.ToDos.Remove(toDoItem);
				await _context.SaveChangesAsync();

				return new ServiceResponseDto
				{
					IsSuccess = true,
					Message = "ToDo item was successfully removed"
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
		private int GetLastIntegerValueAsync(string userId)
		{
			try
			{
				var lastValue = _context.ToDos
					.Where(tdl => tdl.UserId == userId)
					.OrderByDescending(tdl => tdl.Order)
					.Select(tdl => tdl.Order)
					.FirstOrDefault();

				if(lastValue == 0)
				{
					lastValue++;
				}	

				return lastValue;
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
	}
}
