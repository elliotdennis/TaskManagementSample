using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementAPI.DataAccess.Interfaces;
using TaskManagementAPI.Domain;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksAsync()
        {
            return await _context.ProjectTasks.AsNoTracking().ToListAsync();
        }

        public async Task<ProjectTask?> GetTaskByIdAsync(Guid id)
        {
            return await _context.ProjectTasks.FindAsync(id);
        }

        public async Task AddTaskAsync(ProjectTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(ProjectTask task)
        {
            task.LastModified = DateTime.UtcNow;
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(ProjectTask task)
        {
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
