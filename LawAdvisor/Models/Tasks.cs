using System.ComponentModel.DataAnnotations;

namespace LawAdvisor.Models
{
	public class Tasks
	{
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
