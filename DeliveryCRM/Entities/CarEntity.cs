using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
namespace DeliveryCRM.Entities
{
    public class CarEntity
    {
        DataContext _context;
        public CarEntity(DataContext context)
        {
            _context = context;
        }

        public int Id { get; set; }

        public string Model { get; set; }
        public int TypeCargoId { get; set; }
        public TypeCargoEntity typeCargo { 
        get { return _context.TypesCargo.FirstOrDefault(x => x.Id == TypeCargoId); }
        }

        public string NumberCar { get; set; }
    }
}
