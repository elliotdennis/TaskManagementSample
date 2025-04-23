using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ProjectTask>> GetTasksAsync();
        Task<ProjectTask?> GetTaskByIdAsync(Guid id);
        Task AddTaskAsync(ProjectTask task);
        Task UpdateTaskAsync(ProjectTask task);
        Task DeleteTaskAsync(ProjectTask task);
    }
}
