using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveyApp.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string clientRoleName = "client";
            string employeeRoleName = "employee";
            string managerRoleName = "manager";
            

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            string clientEmail = "client@mail.ru";
            string clientPassword = "123456";


            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role clientRole = new Role { Id = 2, Name = clientRoleName };
            Role employeeRole = new Role { Id = 3, Name = employeeRoleName };
            Role managerRole = new Role { Id = 4, Name = managerRoleName };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User clientUser = new User { Id = 2, Email = clientEmail, Password = clientPassword, RoleId = clientRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, clientRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, clientUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
