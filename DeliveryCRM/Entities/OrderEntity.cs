using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public int? DriverEntityId { get; set; }// внешний ключ для водителя
        public DriverEntity DriverEntity { get; set; }

        public int UserEntityId { get; set; }// внешний ключ для заказчика
        public UserEntity UserEntity { get; set; }

        public DateTime DateOrder { get; set; }
        public DateTime DateDelivery { get; set; }
        public string PointStart { get; set; }
        public string PointFinish { get; set; }

        public int Price { get; set; }

        public int StatusOrderEntityId { get; set; } // внешний ключ для статуса
        public StatusOrderEntity StatusOrderEntity { get; set; }

        public int? AdministratorEntityId { get; set; }// внешний ключ для админа
        public ManagerEntity AdministratorEntity { get; set; }



    }
}
