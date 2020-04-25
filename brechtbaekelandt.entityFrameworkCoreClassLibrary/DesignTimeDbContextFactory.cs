using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using brechtbaekelandt.entityFrameworkCoreClassLibrary.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace brechtbaekelandt.entityFrameworkCoreClassLibrary
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GeoDbContext>
    {
        public GeoDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<GeoDbContext>();

            var connectionString = configuration.GetConnectionString("Geo");

            builder.UseSqlServer(connectionString);

            return new GeoDbContext(builder.Options);
        }
    }
}
