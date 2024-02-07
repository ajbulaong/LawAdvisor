using LawAdvisor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LawAdvisor.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }

        public DbSet<Tasks> Tasks { get; set; }
		public DbSet<ToDo> ToDos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Tasks>()
				.HasKey(t => t.Id);
		}
	}
}
