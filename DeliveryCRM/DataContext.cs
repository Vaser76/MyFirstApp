using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using DeliveryCRM.Entities;

namespace DeliveryCRM
{
    public class DataContext: DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        public DbSet<ManagerEntity> Managers { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CarEntity> Cars { get; set; }
        public DbSet<StatusOrderEntity> Statuses { get; set; }
        public DbSet<TypeCargoEntity> TypesCargo { get; set; }
        public DbSet<TokenEmailConfirmationEntity> TokenEmailConfirmations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext()
        {
            //Database.EnsureCreated();
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            TypeCargoEntity small = new TypeCargoEntity {Id = 1, CategoryCargo = "Малогабаритный груз"};
            TypeCargoEntity big = new TypeCargoEntity {Id = 2, CategoryCargo = "крупногабаритный груз"};

            StatusOrderEntity search = new StatusOrderEntity {Id = 1, Status = "Поиск поставщика"};
            StatusOrderEntity load = new StatusOrderEntity {Id = 2, Status = "Выполнение доставки"};
            StatusOrderEntity done = new StatusOrderEntity {Id = 3, Status = "Доставка выполнена"};
            StatusOrderEntity canceled = new StatusOrderEntity {Id = 4, Status = "Доставка отменена"};


            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=123");
        }*/


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string clientRoleName = "client";
            string employeeRoleName = "driver";
            string managerRoleName = "manager";


            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            string clientEmail = "client@mail.ru";
            string clientPassword = "123456";

            // добавляем роли
            RoleEntity adminRole = new RoleEntity { Id = 1, Name = adminRoleName };
            RoleEntity clientRole = new RoleEntity { Id = 2, Name = clientRoleName };
            RoleEntity employeeRole = new RoleEntity { Id = 3, Name = managerRoleName };
            RoleEntity managerRole = new RoleEntity { Id = 4, Name =  employeeRoleName };

            UserEntity adminUser = new UserEntity { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id, Name = "Tom" };
            UserEntity clientUser = new UserEntity { Id = 2, Email = clientEmail, Password = clientPassword, RoleId = clientRole.Id, Name = "Alice" };

            modelBuilder.Entity<RoleEntity>().HasData(new RoleEntity[] { adminRole, clientRole });
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity[] { adminUser, clientUser });
            base.OnModelCreating(modelBuilder);
        }
    }   
}
