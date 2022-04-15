using DeliveyApp.Models;
using DeliveyApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeliveryCRM;
using DeliveryCRM.Entities;
using System.Security.Cryptography;
using System.Text;

namespace DeliveyApp.Controllers
{
    public class AccountController : Controller
    {
        private DataContext _context;
        private SHA256Managed _crypt;

        public AccountController(DataContext context)
        {
            _context = context;
            _crypt = new SHA256Managed();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if(user == null)
                {
                    user = new UserEntity { Email = model.Email, Password = model.Password, Name = "test", Surname = "testovich", EmailConfirmed=false, Salt=model.Salt };
                    RoleEntity role = await _context.Roles.FirstOrDefaultAsync(u => u.Name == "client");
                    if (role != null)
                    {
                        user.Role = role;
                    }
                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var code = GenerateEmailConfrmToken(10);
                    await _context.AddAsync(new TokenEmailConfirmationEntity { Token = code, Activated= false });
                    await _context.SaveChangesAsync();

                    var callbackUrl = Url.Action("ConfirmEmail",
                        "Account",
                        new {userId = user.Id, code = code},
                        protocol : HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                    //await Auntheticate(user);
                    //return RedirectToAction("Account", "Login");
                } else {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult RegisterDriver()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterDriver(RegisterDriverModel model)
        {
            if (ModelState.IsValid)
            {
                DriverEntity driver = await _context.Drivers.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (driver == null)
                {
                    driver = new DriverEntity
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Fio.Split()[1],
                        Surname = model.Fio.Split()[0],
                        Passport = model.Passport,
                        DrivingLicense = model.Numberdriverlicense,
                        EmailConfirmed = true,
                        Salt = model.Salt
                    };

                    RoleEntity role = await _context.Roles.FirstOrDefaultAsync(u => u.Name == "driver");
                    if (role != null)
                    {
                        driver.Role = role;
                    }
                    await _context.AddAsync(driver);
                    await _context.SaveChangesAsync();
                    var code = GenerateEmailConfrmToken(10);
                    await _context.AddAsync(new TokenEmailConfirmationEntity { Token = code, Activated = false });
                    await _context.SaveChangesAsync();

                    /*var callbackUrl = Url.Action("ConfirmEmail",
                        "Account",
                        new { userId = driver.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");*/
                    await Auntheticate(driver);
                    return RedirectToAction("DeliveryHistoryDriver", "Driver");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }





        public static string GenerateEmailConfrmToken(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            System.Text.StringBuilder result = new System.Text.StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int? userId, string code)
        {
            if (userId == null || code == null)
            {
                return Content("Error");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return Content("Error");
            }
            user.EmailConfirmed = true;
            var emailToken = await _context.TokenEmailConfirmations.FirstOrDefaultAsync(x=> x.Token == code);
            emailToken.Activated = true;
            await _context.SaveChangesAsync();
            return Content("Электронная почта успешно подтевеждена");
            
        }



        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool CheckPassInLogin(BaseEntity user, LoginModel model)
        {
            string salt = user.Salt;
            var result = ComputeSha256Hash(model.Password + salt);
            return result == user.Password;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {

            
            
            if (ModelState.IsValid)
            {
                ///Сделать для трёх
                


                BaseEntity toLoginPerson = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (toLoginPerson == null)
                {
                    toLoginPerson = await _context.Managers.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (toLoginPerson !=null && !CheckPassInLogin(toLoginPerson, model))
                        toLoginPerson = null;
                }
                if (toLoginPerson == null)
                {
                    toLoginPerson = await _context.Drivers.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (toLoginPerson != null && !CheckPassInLogin(toLoginPerson, model))
                        toLoginPerson = null;
                }
                if (toLoginPerson == null)
                {
                    toLoginPerson = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (toLoginPerson != null && !CheckPassInLogin(toLoginPerson, model))
                        toLoginPerson = null;
                }

                if (toLoginPerson != null)
                {
                    

                    if (toLoginPerson.EmailConfirmed == false)
                        return Content("Для доступа в систему подтвердите адрес эл. почты");
                    await Auntheticate(toLoginPerson);
                    if (toLoginPerson.RoleId == 1)
                        return RedirectToAction("Admin", "Home");
                    else if (toLoginPerson.RoleId == 3)
                        return RedirectToAction("ActiveOrders", "Manager");
                    else if (toLoginPerson.RoleId == 2)
                        return RedirectToAction("CreateOrder", "User");
                    else if (toLoginPerson.RoleId == 4)
                        return RedirectToAction("DeliveryHistoryDriver", "Driver");
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }



        private async Task Auntheticate(BaseEntity user)
        {
            RoleEntity role;
            if (user.Role == null)
            {
                role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
                user.Role = role;
            } else
            {
                role = user.Role;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType) ;
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
        
    }
}
