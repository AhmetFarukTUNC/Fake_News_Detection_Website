using Microsoft.AspNetCore.Mvc;

using System.Linq;
using FakeNewsMVC.Data;
using FakeNewsMVC.Models;
using YourProject.Models;

namespace YourProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı Users tablosuna kaydet
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Bu email zaten kayıtlı.");
                    return View(model);
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password // İdeal olarak: hashlenmiş şifre saklayın
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Success"] = "Kayıt başarılı! Hoş geldiniz.";
                return RedirectToAction("Register");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    // Giriş başarılı, session oluştur
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserEmail",user.Email);

                    TempData["Success"] = $"Hoş geldin, {user.FullName}!";
                    return RedirectToAction("Index", "News"); // Anasayfaya yönlendir
                }

                ModelState.AddModelError("", "Email veya şifre hatalı.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Session temizle
            return RedirectToAction("Login");
        }



    }
}
