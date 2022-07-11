using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts_V2.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = ContactsV2.db;"); //SQLITE
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => e.Nickname).IsUnique();
            });
        }
    }
}
