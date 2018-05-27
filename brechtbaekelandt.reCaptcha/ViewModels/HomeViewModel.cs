using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brechtbaekelandt.reCaptcha.Models;

namespace brechtbaekelandt.reCaptcha.ViewModels
{
    public class HomeViewModel
    {
        public Person PostPerson { get; set; }

        public Person AjaxPerson { get; set; }
    }
}
