using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class StatusOrderEntity
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public List<StatusOrderEntity> statusOrderEntities { get; set; }// навигационное свойство
    }
}
