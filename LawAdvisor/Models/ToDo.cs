using System.ComponentModel.DataAnnotations;

namespace LawAdvisor.Models
{
	public class ToDo
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }
		public int TaskId { get; set; }
		public Tasks Task { get; set; }
        public int Order { get; set; }
    }
}
