using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.DTO;
using TaskManagement.Models;
using TaskManagement.Utils;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;
        private readonly ILogger<TasksController> _logger;
        private readonly Log log;

        public TasksController(TaskContext context, ILogger<TasksController> logger, Log log)
        {
            _context = context;
            _logger = logger;
            this.log = log;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var name = GetNameFromHeader();
            log.LogMessage($"{name} Made a request for GetTasks");
            try
            {
                var tasks = await _context.Tasks.ToListAsync();
                _logger.LogInformation("Fetched all tasks successfully.");
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var name = GetNameFromHeader();
            log.LogMessage($"{name} Made a request for GetTaskItem");
            try
            {
                var taskItem = await _context.Tasks.FindAsync(id);

                if (taskItem == null)
                {
                    _logger.LogWarning("Task with id {Id} not found.", id);
                    return NotFound($"Task with id {id} not found.");
                }

                _logger.LogInformation("Fetched task with id {Id} successfully.", id);
                return Ok(taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching task with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem([FromBody] TaskItemDto taskItemDto)
        {
            var name = GetNameFromHeader();
            log.LogMessage($"{name} Made a request for PostTaskItem");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for POST request.");
                return BadRequest(ModelState);
            }

            try
            {
                var task = new TaskItem
                {
                    Title = taskItemDto.Title,
                    Description = taskItemDto.Description,
                    DueDate = taskItemDto.DueDate
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Task created successfully with id {Id}.", task.Id);

                return CreatedAtAction(nameof(GetTaskItem), new { id = task.Id }, task);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while creating a task.");
                return StatusCode(StatusCodes.Status500InternalServerError, "A database error occurred while saving the task.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a task.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, [FromBody] TaskItemDto taskItemDto)
        {
            var name = GetNameFromHeader();
            log.LogMessage($"{name} Made a request for PutTaskItem");

            if (id != taskItemDto.Id)
            {
                _logger.LogWarning("Task ID mismatch: {Id} vs {DtoId}.", id, taskItemDto.Id);
                return BadRequest("Task ID mismatch.");
            }

         
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for PUT request.");
                return BadRequest(ModelState);
            }

            try
            {
                var taskItemToUpdate = await _context.Tasks.FindAsync(id);

                if (taskItemToUpdate == null)
                {
                    _logger.LogWarning("Task with id {Id} not found.", id);
                    return NotFound($"Task with id {id} not found.");
                }

                taskItemToUpdate.Title = taskItemDto.Title;
                taskItemToUpdate.Description = taskItemDto.Description;
                taskItemToUpdate.DueDate = taskItemDto.DueDate;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Task with id {Id} updated successfully.", id);

                return Ok(taskItemToUpdate);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while updating task with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "A database error occurred while updating the task.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating task with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var name = GetNameFromHeader();
            log.LogMessage($"{name} Made a request for DeleteTaskItem");

            try
            {
                var taskItem = await _context.Tasks.FindAsync(id);

                if (taskItem == null)
                {
                    _logger.LogWarning("Task with id {Id} not found.", id);
                    return NotFound($"Task with id {id} not found.");
                }

                _context.Tasks.Remove(taskItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Task with id {Id} deleted successfully.", id);

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while deleting task with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "A database error occurred while deleting the task.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting task with id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        private string GetNameFromHeader()
        {
            if (Request.Headers.TryGetValue("name", out var nameHeader))
            {
                return nameHeader.ToString();
            }

            _logger.LogWarning("Name header not found in the request.");
            return "Unknown";
        }
    }
}

