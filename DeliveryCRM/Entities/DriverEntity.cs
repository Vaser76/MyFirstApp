using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class DriverEntity: BaseEntity
    {
        DataContext _context;
        public DriverEntity(DataContext context)
        {
            _context = context;
        }
        public DriverEntity()
        {

        }
        public int CarId { get; set; }
        public CarEntity Car {
            get { return _context.Cars.FirstOrDefault(x => x.Id == CarId); } 
        }



        public DateTime DateOfBirth { get; set; }
        public string Passport { get; set; }
        public string DrivingLicense { get; set; }
        public int Experience { get; set; }
        public int Status { get; set; }

        public List<DriverEntity> DriverEntities { get; set; }// навигационное свойство

    }
}
