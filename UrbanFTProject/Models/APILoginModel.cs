using System.ComponentModel.DataAnnotations;

namespace UrbanFTProject.ToDoList.Web.Models
{
    public class APILoginModel
    {
        [Required(ErrorMessage ="Registered Email is needed to create auth token")]
        public string? Email { get; set; }
    }
}
