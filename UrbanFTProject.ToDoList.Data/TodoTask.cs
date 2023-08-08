using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrbanFTProject.ToDoList.Data
{
    /// <summary>
    /// TodoTask Model class in TodoListApp.Data 
    /// </summary>    
    public class TodoTask
    {
        public TodoTask() { 
        
        }

        public TodoTask(TodoTask todoTask)
        {
            this.Title= todoTask.Title;
            this.Description= todoTask.Description;
            this.Status= todoTask.Status;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Title of the task cannot be empty",AllowEmptyStrings =false)]
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        
        [DefaultValue(TaskStatus.Pending)]
        public TaskStatus Status { get; init; } // "pending", "in progress", or "completed"
        
        public string? UserId { get; set; }
    }

    /// <summary>
    /// Enum to hold if Task is "pending", "in progress", or "completed"
    /// </summary>
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

}