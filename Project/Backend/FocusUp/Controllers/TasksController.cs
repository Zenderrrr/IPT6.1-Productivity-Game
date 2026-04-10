using FocusUp.Application.DTOs;
using FocusUp.Application.Services;
using FocusUp.Common.Exceptions;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FocusUp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskCompletionService _taskCompletionService;
        private readonly UserRepository _userRepository;
        private readonly TaskService _taskService;

        public TasksController(TaskCompletionService taskCompletionService, UserRepository userRepository, TaskService taskService)
        {
            _taskCompletionService = taskCompletionService;
            _userRepository = userRepository;
            _taskService = taskService;
        }

        [HttpGet("")]
        public IActionResult GetAllTasks()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var tasks = _taskService.GetTaskByUserId(userId);
                return Ok(new { tasks });
            }
            catch (TaskNotFoundException)
            {
                return NotFound();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                Task? task = _taskService.GetTaskById(id);

                if (task == null)
                    return NotFound();

                if (task.UserId != userId)
                    return Forbid();

                return Ok(new { task.Title, task.Description, task.Difficulty, task.DurationMin, task.CategoryId, task.DueDate, task.Status });
            }
            catch(Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPost("")]
        public IActionResult CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            if (createTaskRequest == null)
                return BadRequest();

            if (!TryGetUserId(out int userId))
                return Unauthorized();

            if (!Enum.TryParse(createTaskRequest.Difficulty,true, out TaskDifficultyType taskDifficulty))
                return BadRequest("Parsing to TaskDifficultyType failed.");

            try
            {
                var task = new Task(userId, createTaskRequest.Title, createTaskRequest.Description, taskDifficulty, createTaskRequest.DurationMin, Domain.Enums.TaskStatus.Open, createTaskRequest.CategoryId, createTaskRequest.DueDate);

                if (!task.ValidateData())
                    return BadRequest();

                int taskId = _taskService.CreateTask(task);
                return StatusCode(201, new { taskId });
            }
            catch(Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] UpdateTaskRequest updateTaskRequest)
        {
            if (updateTaskRequest == null)
                return BadRequest();

            if (!TryGetUserId(out int userId))
                return Unauthorized();

            if (!Enum.TryParse(updateTaskRequest.Difficulty,true, out TaskDifficultyType taskDifficulty))
                return BadRequest("Parsing to TaskDifficultyType failed.");

            if (!Enum.TryParse(updateTaskRequest.Status,true, out Domain.Enums.TaskStatus taskStatus))
                return BadRequest("Parsing to TaskStatus failed.");

            try
            {
                Task? task = _taskService.GetTaskById(id);

                if (task == null)
                    return NotFound();

                if(task.UserId != userId)
                    return Forbid();

                var updatedTask = new Task(task.UserId, updateTaskRequest.Title, updateTaskRequest.Description, taskDifficulty, updateTaskRequest.DurationMin, taskStatus, updateTaskRequest.CategoryId, updateTaskRequest.DueDate);

                if (!updatedTask.ValidateData())
                    return BadRequest("Task data is invalid.");
                
                _taskService.UpdateTask(updatedTask);
                return Ok(new { updatedTask.Title, updatedTask.Description, updatedTask.Difficulty, updatedTask.DurationMin, updatedTask.CategoryId, updatedTask.DueDate, updatedTask.Status });
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                Task? task = _taskService.GetTaskById(id);

                if (task == null)
                    return NotFound();

                if (task.UserId != userId)
                    return Forbid();

                _taskService.DeleteTask(task.Id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPost("{id}/complete")]
        public IActionResult CompleteTask(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                _taskCompletionService.CompleteTask(id, userId);
                return Ok();
            }
            catch (TaskNotFoundException)
            {
                return NotFound();
            }
            catch (TaskAlreadyCompletedException)
            {
                return Conflict();
            }
            catch (UnauthorizedTaskAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            var userIdClaim =
                User.FindFirst("sub")?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return false;

            return int.TryParse(userIdClaim, out userId);
        }
    }
}