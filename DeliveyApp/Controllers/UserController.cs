using Microsoft.AspNetCore.Mvc;
using DeliveryCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryCRM.Entities;
using DeliveyApp.Services;
using DeliveyApp.Models;

namespace DeliveyApp.Controllers
{
    public class UserController : Controller
    {
        private DataContext _context;
        private CostCalculator _costCalculator;

        public UserController(DataContext context)
        {
            _context = context;
            _costCalculator = new CostCalculator();
        }

        public IActionResult DeliveryHistory()
        {
            var idClient =_context.Users.FirstOrDefault(w => w.Email == User.Identity.Name);
            return View(_context.Orders.Where(w => w.UserEntity == idClient).OrderBy(x => x.DateOrder).ToList());
        }


        public IActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(string startPoint, string finishPoint)
        {
            var price = _costCalculator.Calculate(startPoint, finishPoint);
            var idClient = _context.Users.FirstOrDefault(w => w.Email == User.Identity.Name);
            OrderEntity newOrder = new OrderEntity
            {
                PointStart = startPoint,
                PointFinish = finishPoint,
                UserEntity = idClient,
                StatusOrderEntityId = 1,
                DateOrder = DateTime.Now,
                Price = price
        };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckPrice(string startPoint, string finishPoint)
        {
            //мб асинк
            var price = _costCalculator.Calculate(startPoint, finishPoint);
            var order = new OrderCost() { StartPoint = startPoint, FinishPoint = finishPoint, Price = price};
            return new JsonResult(order);
        }

    }
}
