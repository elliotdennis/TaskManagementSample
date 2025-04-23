using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Application.DTOs;
using TaskManagementAPI.Application.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all tasks.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddTask([FromBody] NewTaskDto dto)
        {
            try
            {
                await _taskService.AddTaskAsync(dto);
                return CreatedAtAction(nameof(GetAllTasks), new { }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating new task.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<TaskDto>> UpdateTask([FromBody] TaskDto dto)
        {
            try
            {
                var task = await _taskService.UpdateTaskAsync(dto);
                return Ok(task);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict occurred while updating task.");
                return Conflict("Concurrency conflict occurred. Please refresh and try again.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while updating task.");
                return StatusCode(500, "Database update error. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating task.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting task.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
