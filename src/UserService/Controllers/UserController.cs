using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}