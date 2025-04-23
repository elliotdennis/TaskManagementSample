using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.Domain
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public virtual DbSet<ProjectTask> ProjectTasks { get; set; }
    }
}
