using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.DbContext;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Extensions;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}