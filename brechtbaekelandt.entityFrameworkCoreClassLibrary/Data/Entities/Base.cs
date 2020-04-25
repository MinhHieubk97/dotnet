using System.ComponentModel.DataAnnotations;

namespace brechtbaekelandt.entityFrameworkCoreClassLibrary.Data.Entities
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
