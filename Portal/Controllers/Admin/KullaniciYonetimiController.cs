using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Models;
using Portal.Helpers;
using Portal.ViewModels;
using Portal.Controllers.Admin;
using Microsoft.EntityFrameworkCore;
using Portal.Models.Admin;
using Portal.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portal.Controllers.Admin
{
    [Route("Admin/KullaniciYonetimi")]
    public class KullaniciYonetimiController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public KullaniciYonetimiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ListUsers Action
        [HttpGet("ListUsers")]
        public IActionResult ListUsers()
        {
            var users = _context.Users.ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(users);
        }


        [HttpGet("AddUser")]
        public IActionResult AddUser()
        {
            // KanGrubu listesini al ve ViewBag ile View'e gönder
            ViewBag.KanGrubus = new SelectList(_context.KanGrubus, "Id", "Ad");
            return View(new AddUserViewModel());
        }



        [HttpPost("AddUser")]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(AddUserViewModel model, IFormFile ProfileImage)
        {
            if (ModelState.IsValid)
            {
                // Hem TcKimlik hem de Email'i kontrol eden sorgu
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email || u.TcKimlik == model.TcKimlik);

                if (existingUser != null)
                {
                    // Hangi alanın çakıştığını bul ve ona göre hata ekle
                    if (existingUser.Email == model.Email)
                    {
                        ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                    }
                    if (existingUser.TcKimlik == model.TcKimlik)
                    {
                        ModelState.AddModelError("TcKimlik", "Bu TC Kimlik numarası zaten kullanılıyor.");
                    }
                    return View(model);
                }

                var hashedPassword = HashHelper.HashPassword(model.Password);

                byte[] imageData = null;
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        ProfileImage.CopyTo(ms);
                        imageData = ms.ToArray();
                    }
                }

                var user = new User
                {
                    TcKimlik = model.TcKimlik,
                    BirthDate = model.BirthDate,
                    FullName = model.FullName ?? string.Empty,
                    Email = model.Email ?? string.Empty,
                    Password = hashedPassword,
                    About = model.About ?? string.Empty,
                    Address = model.Address ?? string.Empty,
                    Company = model.Company ?? string.Empty,
                    JobTitle = model.JobTitle ?? string.Empty,
                    Country = model.Country ?? string.Empty,
                    Phone = model.Phone,
                    Twitter = model.Twitter ?? string.Empty,
                    Facebook = model.Facebook ?? string.Empty,
                    Instagram = model.Instagram ?? string.Empty,
                    Puan = model.Puan ?? "0",
                    MedeniDurum = model.MedeniDurum ?? string.Empty,
                    Cinsiyet = model.Cinsiyet ?? string.Empty,
                    OgrenimDurumu = model.OgrenimDurumu ?? string.Empty,
                    KanGrubuId = model.KanGrubuId,
                    ProfileImage = imageData
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                var role = _context.Roles.FirstOrDefault(r => r.Name == "User");
                if (role != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    _context.UserRoles.Add(userRole);
                }

                var homeIndexPermission = _context.Permissions.FirstOrDefault(p => p.Controller == "Home" && p.Action == "Index");
                if (homeIndexPermission != null)
                {
                    var rolePermission = _context.RolePermissions
                        .SingleOrDefault(rp => rp.RoleId == role.Id && rp.PermissionId == homeIndexPermission.Id);

                    if (rolePermission == null)
                    {
                        rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = homeIndexPermission.Id
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                return RedirectToAction("ListUsers");
            }

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }

            return View(model);
        }


        [HttpGet("EditUser/{id}")]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.KanGrubus = new SelectList(_context.KanGrubus, "Id", "Ad");

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                About = user.About,
                Address = user.Address,
                Company = user.Company,
                JobTitle = user.JobTitle,
                Country = user.Country,
                Phone = user.Phone,
                Twitter = user.Twitter,
                Facebook = user.Facebook,
                Instagram = user.Instagram,
                Puan = user.Puan,
                ProfileImage = user.ProfileImage,
                TcKimlik = user.TcKimlik,
                BirthDate = user.BirthDate,
                Cinsiyet = user.Cinsiyet,
                OgrenimDurumu = user.OgrenimDurumu,
                KanGrubuId = user.KanGrubuId // Kan Grubu Id'sini ViewModel'e aktarıyoruz

            };
            return View(model);
        }
        [HttpPost("EditUser/{id}")]
        public IActionResult EditUser(EditUserViewModel model, IFormFile? ProfileImage)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.About = model.About;
                user.Address = model.Address;
                user.Company = model.Company;
                user.JobTitle = model.JobTitle;
                user.Country = model.Country;
                user.Phone = model.Phone;
                user.Twitter = model.Twitter;
                user.Facebook = model.Facebook;
                user.Instagram = model.Instagram;
                user.Puan = model.Puan;
                user.TcKimlik = model.TcKimlik;
                user.BirthDate = model.BirthDate;
                user.Cinsiyet = model.Cinsiyet;
                user.MedeniDurum = model.MedeniDurum;
                user.KanGrubuId = model.KanGrubuId;


                // OgrenimDurumu alanını ekleyin
                user.OgrenimDurumu = model.OgrenimDurumu;

                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        ProfileImage.CopyTo(ms);
                        user.ProfileImage = ms.ToArray();
                    }
                }

                _context.Users.Update(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction("ListUsers");
            }
            // Kan Grubu dropdown için verileri tekrar gönderiyoruz
            ViewBag.KanGrubus = new SelectList(_context.KanGrubus, "Id", "Ad");
            // ModelState geçerli değilse mevcut resmi ViewModel'e tekrar yükle
            model.ProfileImage = _context.Users
                .Where(u => u.Id == model.Id)
                .Select(u => u.ProfileImage)
                .FirstOrDefault();

            return View(model);
        }



        [HttpGet("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("ConfirmDeleteUser")]
        public IActionResult ConfirmDeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("ListUsers");
        }



    }

}

