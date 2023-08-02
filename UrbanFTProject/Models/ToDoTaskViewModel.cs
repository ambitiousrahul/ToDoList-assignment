using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UrbanFTProject.ToDoList.Data;
using TaskStatus = UrbanFTProject.ToDoList.Data.TaskStatus;

namespace UrbanFTProject.ToDoList.Web.Models
{
    public class ToDoTaskViewModel
    {
        [Required(ErrorMessage = "Title of the task cannot be empty", AllowEmptyStrings = false)]
        public string Title { get; init; } = default!;
        public string? Description { get; init; }

        [DefaultValue(TaskStatus.Pending)]
        public TaskStatus Status { get; init; }

        public string? TaskAssignee { get; init; }
    }
}
