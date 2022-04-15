using Microsoft.AspNetCore.Mvc;
using DeliveryCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryCRM.Entities;
using Microsoft.AspNetCore.Authorization;

namespace DeliveyApp.Controllers
{
    [Authorize(Policy = "Driver")]
    public class DriverController : Controller
    {
        private DataContext _context;

        public DriverController(DataContext context)
        {
            _context = context;
        }

        public IActionResult DeliveryHistoryDriver()
        {
            var idDriver = _context.Drivers.FirstOrDefault(w => w.Email == User.Identity.Name);
            return View(_context.Orders.Where(w => w.DriverEntity == idDriver).OrderBy(x => x.DateOrder).ToList());
        }
    }
}
