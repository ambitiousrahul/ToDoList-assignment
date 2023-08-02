using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UrbanFTProject.Repository;
using UrbanFTProject.ToDoList.Data;
using UrbanFTProject.ToDoList.Web.Middlewares;
using UrbanFTProject.ToDoList.Web.Models;


namespace UrbanFTProject.ToDoList.Web.Controllers
{
    // Controllers/TasksController.cs in TodoListApp.Web
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[ServiceFilter(typeof(LoginUrlAuthorizationFilter))]
    public class ToDoTasksController : Controller
    {
        private readonly IRepository<TodoTask> _taskRepository;
        private readonly IUserRepository _userRepository;

        public ToDoTasksController(IRepository<TodoTask> taskRepository,IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTasks()
        {
            var toDoTasks = await _taskRepository.GetAllAsync();
            return toDoTasks?.Count() > 0 ? Ok(toDoTasks) : NotFound();            
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoTask>> GetTask(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task is null ? NotFound() : Ok(task);            
        }

        [HttpPost]
        [Authorize]
        //[ServiceFilter(typeof(LoginUrlAuthorizationFilter))]
        public async Task<ActionResult<TodoTask>> AddTask([FromBody]ToDoTaskViewModel task)
        {
            string? userId = null;
            if (!string.IsNullOrWhiteSpace(task.TaskAssignee))
            {
                var userDetails =await _userRepository.GetUserByEmail(task.TaskAssignee);
                userId = userDetails.Id;
            }

            TodoTask todoTask = new()
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                UserId = userId
            };
            
            var addedTask= await _taskRepository.AddAsync(todoTask);            

            return CreatedAtAction(nameof(GetTask), new { id = addedTask.Id }, addedTask);
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
