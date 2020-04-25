using System.Collections.Generic;

namespace brechtbaekelandt.entityFrameworkCoreClassLibrary.Data.Entities
{
    public class Country : Base
    {

        public Country()
        {
            
        }

        public Country(string isoCode, string name, int id)
        {
            this.IsoCode = isoCode;
            this.Name = name;
            this.Id = id;
        }

        public string IsoCode { get; set; }

        public virtual IEnumerable<State> States { get; set; }
    }
}
