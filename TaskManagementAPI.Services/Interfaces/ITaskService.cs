using TaskManagementAPI.Application.DTOs;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task AddTaskAsync(NewTaskDto dto);
        Task<TaskDto> UpdateTaskAsync(TaskDto dto);
        Task DeleteTaskAsync(Guid id);
    }
}
