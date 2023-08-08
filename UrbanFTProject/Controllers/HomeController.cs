using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanFTProject.ToDoList.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
