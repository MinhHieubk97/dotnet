using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities
{
    public abstract class LivingBeing
    {
        [Key]
        public int Id { get; set; }

        public DateTime Birthday { get; set; }
        
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var bday = this.Birthday;

                var age = today.Year - bday.Year;

                if (today < bday.AddYears(age))
                {
                    age--;
                }

                return age;
            }
            set { }
        }
    }
}