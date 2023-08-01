using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UrbanFTProject.Repository;
using UrbanFTProject.ToDoList.Data;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    // Controllers/TasksController.cs in TodoListApp.Web
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly IRepository<TodoTask> _taskRepository;

        public ToDoTasksController(IRepository<TodoTask> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTasks()
        {
            return Ok(await _taskRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoTask>> GetTask(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<TodoTask>> AddTask(TodoTask task)
        {
            await _taskRepository.AddAsync(task);            

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody]TodoTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            await _taskRepository.UpdateAsync(task);                
          
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if (taskId == 0)
            {
                return BadRequest("TaskId cannot be zero");
            }            

           var dataState= await _taskRepository.DeleteAsync(taskId);

            if (dataState != DataRowState.Deleted)
            {
                return NotFound("No Task was found in system for the specified task ID");
            }
            else
            {
                return Ok();
            }
        }

    }

}
