using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DeliveryCRM;
using DeliveryCRM.Entities;

namespace DeliveyApp.ViewModels
{
    public class ActiveOrdersModel
    {
        public List<OrderEntity> listActiveOrders;
        DataContext _context;
        public ActiveOrdersModel(DataContext context)
        {
            _context = context;
        }

       /* public List<OrderEntity> FindActiveOrders()
        {
            listActiveOrders = await _context.FindAsync();
            return null;
        }*/
    }
}
