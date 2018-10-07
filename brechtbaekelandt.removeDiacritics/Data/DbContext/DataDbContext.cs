using brechtbaekelandt.removeDiactritics.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace brechtbaekelandt.removeDiactritics.Data.DbContext
{
    public class DataDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options)
            : base(options)
        {

        }

        public DbSet<Word> Words { get; set; }

        [DbFunction("RemoveDiacritics", "dbo")]
        public static string RemoveDiacritics(string input)
        {
            throw new NotImplementedException("This method can only be called in LINQ-to-Entities!");
        }
    }
}
