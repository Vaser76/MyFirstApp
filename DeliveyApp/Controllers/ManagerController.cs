using Microsoft.AspNetCore.Mvc;
using DeliveryCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveyApp.Controllers
{
    public class ManagerController : Controller
    {
        private DataContext _context;

        public ManagerController(DataContext context)
        {
            _context = context;
        }

        public IActionResult ActiveOrders()
        {
            return  View(_context.Orders.Where(x => x.StatusOrderEntityId == 1).OrderBy(x => x.DateOrder).ToList());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AssignDriver(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View(_context.Drivers.Where(x => x.Status == 1).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AssignDriverAsync(int orderId, int driverId)
        {
            ViewBag.OrderId = orderId;
            var order = _context.Orders.FirstOrDefault(x=> x.Id == orderId); 
            order.DriverEntityId = driverId;
            order.AdministratorEntityId = 1;
            order.StatusOrderEntityId = 2;
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == driverId);
            driver.Status = 2;
            await _context.SaveChangesAsync();
            return View(_context.Drivers.Where(x => x.Status == 1).ToList());
        }

        [HttpGet]
        public IActionResult AssignedOrders()
        {
            var idManager = _context.Managers.FirstOrDefault(w => w.Email == User.Identity.Name);
            return View(_context.Orders.Where(x => x.AdministratorEntityId == idManager.Id && x.StatusOrderEntityId != 1).ToList());
        }

    }
}
