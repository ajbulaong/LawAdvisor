using LawAdvisor.Models;
using Microsoft.AspNetCore.Mvc;

namespace LawAdvisor.Interface
{
	public interface IToDoService
	{
		public Task<List<ToDo>> GetToDoList(string userId);
		public Task<ServiceResponseDto> AssignTask(ToDo request);
		public Task<ServiceResponseDto> RearrangeTask(int id, int newPos);
		public Task<ServiceResponseDto> RemoveTask(int id);
	}
}
