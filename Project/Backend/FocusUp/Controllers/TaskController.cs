using FocusUp.Application.Services;
using FocusUp.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskCompletionService _taskCompletionService;

        public TaskController(TaskCompletionService taskService)
        {
            _taskCompletionService = taskService;
        }

        [HttpPost("{id}/complete")]
        public IActionResult CompleteTask(int id)
        {
            int userId = 1; // temporäre Testgründe

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
    }
}