using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities
{
    public class Person : LivingBeing
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }
        
        public string Name
        {
            get
            {
                return $"{this.Firstname} {this.Lastname}";
            }
            set { }
        }

        public virtual ICollection<Animal> Pets { get; set; }
    }
}