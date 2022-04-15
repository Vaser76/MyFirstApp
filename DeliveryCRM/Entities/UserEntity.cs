using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class UserEntity: BaseEntity
    {
        public string NumberPhone { get; set; }
        public List<UserEntity> userEntities { get; set; }// навигационное свойство
    }
}
