using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    public partial class CdyhcdDBContext : DbContext
    {
        public CdyhcdDBContext(IOnDatabaseConfiguring configuring, DbContextOptions options) : base(options)
        {
            DatabaseConfiguring = configuring;
        }

        public IOnDatabaseConfiguring DatabaseConfiguring { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (DatabaseConfiguring != null)
            {
                DatabaseConfiguring.OnConfiguring(optionsBuilder);
            }
        }
    }
}
