using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Data.DbContext
{
    public class DataDbContext : System.Data.Entity.DbContext
    {
        public DataDbContext() : base("enableODataQueryOnComputedProperties")
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Animal> Animals { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Person>().Ignore(e => e.Age);
            modelBuilder.Entity<Person>().Ignore(e => e.Name);
            modelBuilder.Entity<Animal>().Ignore(e => e.Age);
        }

       

    }
}