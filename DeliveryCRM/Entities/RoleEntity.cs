using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserEntity> Users { get; set; }
        public List<ManagerEntity> Managers { get; set; }
        public List<DriverEntity> Drivers { get; set; }

        public RoleEntity()
        {
            Users = new List<UserEntity>();
            Drivers = new List<DriverEntity>();
            Managers = new List<ManagerEntity>();
        }
    }
}
