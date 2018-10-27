using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities
{
    public class Animal : LivingBeing
    {
        public string Name { get; set; }

        public Species Species { get; set; }

        public virtual Person Owner { get; set; }
    }

    public enum Species
    {
        Cat,
        Dog,
        Horse,
        Chicken
    }
}