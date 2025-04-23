using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DataAccess.Repositories;
using TaskManagementAPI.Domain;
using TaskManagementAPI.Domain.Models;

namespace TaskManagementAPI.DataAccess.Tests.Tasks
{
    public class TaskRepositoryTests
    {
        [Fact]
        public async Task GetTasksAsync_ShouldReturnAllTasks()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TaskManagementContext(options);
            var taskRepository = new TaskRepository(context);

            var task1 = new ProjectTask { Id = Guid.NewGuid(), Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do", Version = new byte[] { 1 } };
            var task2 = new ProjectTask { Id = Guid.NewGuid(), Title = "Task 2", Assignee = "Jane Doe", LastModified = DateTime.Now.AddSeconds(-1000), Status = "In Progress", Version = new byte[] { 1 } };
            context.ProjectTasks.AddRange(task1, task2);
            context.SaveChanges();

            var tasks = await taskRepository.GetTasksAsync();

            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnOneTask()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TaskManagementContext(options);
            var taskRepository = new TaskRepository(context);

            var task = new ProjectTask { Id = Guid.NewGuid(), Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do", Version = new byte[] { 1 } };
            context.ProjectTasks.Add(task);
            context.SaveChanges();

            var result = await taskRepository.GetTaskByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldAddTask()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TaskManagementContext(options);
            var taskRepository = new TaskRepository(context);

            var task = new ProjectTask { Id = Guid.NewGuid(), Title = "New Task", Assignee = "John Smith", LastModified = DateTime.Now, Status = "To Do", Version = new byte[] { 1 } };
            await taskRepository.AddTaskAsync(task);

            var result = await taskRepository.GetTaskByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TaskManagementContext(options);
            var taskRepository = new TaskRepository(context);

            var task = new ProjectTask { Id = Guid.NewGuid(), Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do", Version = new byte[] { 1 } };
            context.ProjectTasks.Add(task);
            context.SaveChanges();

            task.Title = "Updated Task";
            await taskRepository.UpdateTaskAsync(task);

            var result = await taskRepository.GetTaskByIdAsync(task.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Title);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TaskManagementContext(options);
            var taskRepository = new TaskRepository(context);

            var task = new ProjectTask { Id = Guid.NewGuid(), Title = "Task 1", Assignee = "John Smith", LastModified = DateTime.Now.AddDays(-1), Status = "To Do", Version = new byte[] { 1 } };
            context.ProjectTasks.Add(task);
            context.SaveChanges();

            await taskRepository.DeleteTaskAsync(task);

            var result = await taskRepository.GetTaskByIdAsync(task.Id);
            Assert.Null(result);
        }
    }
}
