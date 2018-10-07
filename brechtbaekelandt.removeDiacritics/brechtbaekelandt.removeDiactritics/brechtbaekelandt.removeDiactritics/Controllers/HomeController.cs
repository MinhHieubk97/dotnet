using Microsoft.AspNetCore.Mvc;

namespace brechtbaekelandt.removeDiactritics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
