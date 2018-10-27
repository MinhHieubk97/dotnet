using brechtbaekelandt.enableODataQueryOnComputedProperties.Data.Entities;

namespace brechtbaekelandt.enableODataQueryOnComputedProperties.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<brechtbaekelandt.enableODataQueryOnComputedProperties.Data.DbContext.DataDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(brechtbaekelandt.enableODataQueryOnComputedProperties.Data.DbContext.DataDbContext context)
        {
            context.Persons.AddOrUpdate(new Person(){ Firstname = "Bill", Lastname = "Gates", Birthday = new DateTime(1955, 10, 28)});
            context.Persons.AddOrUpdate(new Person() { Firstname = "Satya", Lastname = "Nadella", Birthday = new DateTime(1969, 8, 19) });
            context.Persons.AddOrUpdate(new Person() { Firstname = "Scott", Lastname = "Hanselman", Birthday = new DateTime(1970, 1, 1) });
            context.Persons.AddOrUpdate(new Person() { Firstname = "Scott", Lastname = "Guthrie", Birthday = new DateTime(1975, 2, 6) });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
