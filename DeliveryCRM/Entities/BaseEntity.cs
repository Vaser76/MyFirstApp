﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class BaseEntity: IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Salt { get; set; }

    }
}
