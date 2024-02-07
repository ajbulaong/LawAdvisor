using LawAdvisor.Models;

namespace LawAdvisor.Interface
{
	public interface ITaskService
	{
		public Task<List<Tasks>> GetTasks();
		public Task<Tasks> GetTaskById(int taskId);
		public Task<ServiceResponseDto> CreateTask(Tasks request);
		public Task<ServiceResponseDto> UpdateTask(Tasks request);
	}
}
