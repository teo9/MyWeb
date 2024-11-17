using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.EntityFramework
{
    public class MyWebDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MyWebDBContext(
            IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        public DbSet<Core.Administration.UserPermissions.UserPermission> UserPermissions { get; set; }
        public DbSet<Core.Administration.Users.User> Users { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultSQLServer"));
            base.OnConfiguring(optionsBuilder);
        } 
    }
}
