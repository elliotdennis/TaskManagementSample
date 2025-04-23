using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Application.DTOs;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.DataAccess.Interfaces;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetTasksAsync();
            return tasks.Select(GenerateDto);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            return task != null ? GenerateDto(task) : null;
        }

        public async Task AddTaskAsync(NewTaskDto dto)
        {
            var task = new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Assignee = dto.Assignee,
                Status = dto.Status,
                LastModified = DateTime.UtcNow
            };
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task<TaskDto> UpdateTaskAsync(TaskDto dto)
        {
            var task = await _taskRepository.GetTaskByIdAsync(dto.Id);

            if (task == null)
            {
                throw new Exception("Task not found");
            }

            // does not work properly with In Memory DB
            task.Version = dto.Version;

            // using this for in memory DB concurrency
            if (task.LastModified != dto.LastModified)
                throw new DbUpdateConcurrencyException("The task has been modified by another user.");

            task.Title = dto.Title;
            task.Assignee = dto.Assignee;
            task.Status = dto.Status;

            await _taskRepository.UpdateTaskAsync(task);

            var updated = await _taskRepository.GetTaskByIdAsync(dto.Id);
            return updated != null ? GenerateDto(updated) : throw new Exception("Task not found");
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            await _taskRepository.DeleteTaskAsync(task);
        }

        private static TaskDto GenerateDto(ProjectTask task)
        {
            return new TaskDto
            (
                task.Id,
                task.Title,
                task.Assignee,
                task.Status,
                task.LastModified,
                task.Version
            );
        }
    }
}
