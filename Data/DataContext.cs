using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyContactsAPI.Extensions;
using MyContactsAPI.Maps;
using MyContactsAPI.Models.ContactModels;
using MyContactsAPI.Models.UserModels;
using System;

namespace ContactsManage.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }

        public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
        {
            public DataContext CreateDbContext(string[] args)
            {
                try
                {
                    if (string.IsNullOrEmpty(Configuration.Database.ConnectionString))
                    {
                        throw new InvalidOperationException("Database connection string is not configured.");
                    }

                    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                    optionsBuilder.UseNpgsql(Configuration.Database.ConnectionString);

                    return new DataContext(optionsBuilder.Options);
                }
                catch (Exception ex)
                {
                    // Add error handling here, such as logging or throwing a custom exception
                    throw new InvalidOperationException("Failed to create database context.", ex);
                }
            }
        }
    }
}
