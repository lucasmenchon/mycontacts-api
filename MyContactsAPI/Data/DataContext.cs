using MyContactsAPI.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Models.ContactModels;

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
                string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string environmentSuffix = environmentName == "Development" ? ".Development" : "";
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings{environmentSuffix}.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

                if (environmentName.Equals("Development"))
                {
                    optionsBuilder.UseNpgsql(configuration.GetConnectionString("LocalDb"));
                }
                else
                {
                    optionsBuilder.UseNpgsql(configuration.GetConnectionString("HostDb"));
                }

                return new DataContext(optionsBuilder.Options);
            }
        }
    }
}