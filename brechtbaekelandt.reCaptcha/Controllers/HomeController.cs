using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using brechtbaekelandt.reCaptcha.Attributes;
using brechtbaekelandt.reCaptcha.Models;
using brechtbaekelandt.reCaptcha.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace brechtbaekelandt.reCaptcha.Controllers
{
    public class HomeController : Controller
    {
        private const string _secretKey = "6LfCq1gUAAAAAOgHTYuQkBTXcgLb9veO-FGKAerv";

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateReCaptcha(secretKey: _secretKey)]
        [ValidateAntiForgeryToken]
        public IActionResult Post([Bind(Prefix = "PostPerson")]Person model)
        {
            if (this.ModelState.IsValid)
            {
                this.ViewData["PostSuccessMessage"] = $"You sucessfully added {model.FirstName} {model.LastName}.";
            }
            else
            {
                this.ViewData["PostErrorMessage"] = $"You must complete all the fields and ReCaptcha correctly.";
            }
            
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [Route("home/ajaxpost")]
        [ValidateReCaptcha(secretKey: _secretKey)]
        public IActionResult AjaxPost([FromBody]Person model)
        {
            if (this.ModelState.IsValid)
            {
                return this.Json(new {message = $"You sucessfully added {model.FirstName} {model.LastName}.", person = model});
            }

            return this.BadRequest(new { message = $"You must complete all the fields and ReCaptcha correctly.", person = model });
        }
    }
}
