using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagementAPI.Application.DTOs;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Application.Services;
using TaskManagementAPI.DataAccess.Interfaces;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.Application.Tests.Tasks
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly ITaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnAllTasks()
        {
            var tasks = new List<ProjectTask>
            {
                new ProjectTask { Id = Guid.NewGuid(), Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do" },
                new ProjectTask { Id = Guid.NewGuid(), Title = "Task 2", Assignee = "Jane Doe", LastModified = DateTime.Now.AddSeconds(-1000), Status = "In Progress" }
            };

            _taskRepositoryMock.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(tasks);

            var result = await _taskService.GetAllTasksAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnTask_WhenExists()
        {
            var taskId = Guid.NewGuid();
            var task = new ProjectTask { Id = taskId, Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do" };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(task);

            var result = await _taskService.GetTaskByIdAsync(taskId);

            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnNull_WhenNotExists()
        {
            var taskId = Guid.NewGuid();
            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync((ProjectTask?)null);
            var result = await _taskService.GetTaskByIdAsync(taskId);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTask_ShouldSuccessfullyAdd()
        {
            var taskDto = new NewTaskDto("New Task", "", "To Do");
            await _taskService.AddTaskAsync(taskDto);
            _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(It.IsAny<ProjectTask>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTask_ShouldSuccessfullyUpdate()
        {
            var taskId = Guid.NewGuid();
            var taskDate = DateTime.Now.AddDays(-1);
            var taskDto = new TaskDto(taskId, "Updated Task", "John Doe", "In Progress", taskDate, new byte[] { 1 });
            var existingTask = new ProjectTask { Id = taskId, Title = "Old Task", Assignee = "Jane Doe", LastModified = taskDate, Status = "To Do", Version = new byte[] { 1 } };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(existingTask);
            var updated = await _taskService.UpdateTaskAsync(taskDto);

            _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<ProjectTask>()), Times.Once);
            Assert.Equal(taskDto.Title, updated.Title);
        }

        [Fact]
        public async Task UpdateTask_ShouldThrowException_WhenTaskNotFound()
        {
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto(taskId, "Updated Task", "John Doe", "In Progress", DateTime.Now, new byte[] { 1 });

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync((ProjectTask?)null);

            await Assert.ThrowsAsync<Exception>(() => _taskService.UpdateTaskAsync(taskDto));
        }

        [Fact]
        public async Task UpdateTask_ShouldThrowException_WhenConcurrencyFails()
        {
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto(taskId, "Updated Task", "John Doe", "In Progress", DateTime.Now, new byte[] { 1 });
            var existingTask = new ProjectTask { Id = taskId, Title = "Old Task", Assignee = "Jane Doe", LastModified = DateTime.Now.AddSeconds(-200), Status = "To Do", Version = new byte[] { 2 } };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(existingTask);
            _taskRepositoryMock.Setup(repo => repo.UpdateTaskAsync(It.IsAny<ProjectTask>())).ThrowsAsync(new DbUpdateConcurrencyException());

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _taskService.UpdateTaskAsync(taskDto));
        }

        [Fact]
        public async Task DeleteTask_ShouldSuccessfullyDelete()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new ProjectTask { Id = taskId, Title = "Old Task", Assignee = "Jane Doe", LastModified = DateTime.Now.AddDays(-1), Status = "To Do", Version = new byte[] { 1 } };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(existingTask);

            await _taskService.DeleteTaskAsync(taskId);

            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(It.IsAny<ProjectTask>()), Times.Once);
        }
    }
}
