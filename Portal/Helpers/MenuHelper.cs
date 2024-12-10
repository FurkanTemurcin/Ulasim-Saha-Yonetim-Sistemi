using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Portal.ViewModels;
using Portal.Models.Admin;
using Portal.ViewModels.Admin;

namespace Portal.Helpers
{
    public class MenuHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // Görünmesini istemediğimiz actionlar
        private readonly HashSet<string> _excludedActions = new HashSet<string> 
        {
            "SubmitAnswer",   "Details","Ekle","ShowImage","ChangePassword","Update","EditUser","DeleteUser",
            "ConfirmDeleteUser", "Logout", "Delete", "Sil", "SilOnayla", "DeleteConfirmed", 
            "Edit", "Duzenle", "AccessDenied" ,"GetFilteredPlates", "Index2", "GetFoto", "GetVideo","AddDenetimGecmisi","GetPlakaCezaVerileri", "GetTutanakVerileri",
        "Create", "CreateCeza", "DenetimOlustur","GetPlates","GetTutanakDataByPlaka"
        };

        // Görünmesini istemediğimiz controllerlar
        private readonly HashSet<string> _excludedControllers = new HashSet<string> 
        { 
            "Menu", "Profile" 
        };

        // Görünmesini istemediğimiz klasörler
        private readonly HashSet<string> _excludedFolders = new HashSet<string> 
        { 
             "Anasayfa" 
        };

        public MenuHelper(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<MenuItemViewModel> GetAccessibleMenus()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return new List<MenuItemViewModel>();
            }

            var userRoles = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToList();

            var permissions = _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission)
                .Distinct()
                .Where(p => !_excludedActions.Contains(p.Action))
                .ToList();

            var controllerNamespaces = GetControllerNamespaces();
            var controllerDisplayNames = GetControllerDisplayNames();
            var actionDisplayNames = GetActionDisplayNames();

            var menuItems = permissions
                .Where(permission => !_excludedControllers.Contains(permission.Controller)) // Controller'ları filtrele
                .Select(permission => new MenuItemViewModel
                {
                    OriginalController = permission.Controller,
                    OriginalAction = permission.Action,
                    DisplayController = controllerDisplayNames.ContainsKey(permission.Controller) 
                        ? controllerDisplayNames[permission.Controller] 
                        : permission.Controller,
                    DisplayAction = actionDisplayNames.ContainsKey(permission.Action) 
                        ? actionDisplayNames[permission.Action] 
                        : permission.Action,
                    ControllerFolder = GetControllerFolder(permission.Controller, controllerNamespaces)
                })
                .Where(menuItem => !_excludedFolders.Contains(menuItem.ControllerFolder)) // Klasörleri filtrele
                .ToList();

            return menuItems;
        }

        private Dictionary<string, string> GetControllerNamespaces()
        {
            var controllerNamespaces = new Dictionary<string, string>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var controllers = assembly.GetTypes()
                        .Where(type => typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(type) && !type.IsAbstract)
                        .ToList();

                    foreach (var controller in controllers)
                    {
                        var controllerName = controller.Name.Replace("Controller", "");
                        if (!controllerNamespaces.ContainsKey(controllerName))
                        {
                            controllerNamespaces[controllerName] = controller.Namespace;
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Handle the exception for reflection load issues
                    var loaderExceptions = ex.LoaderExceptions;
                    // Log or handle the loader exceptions as needed
                }
            }

            return controllerNamespaces;
        }

        private string GetControllerFolder(string controllerName, Dictionary<string, string> controllerNamespaces)
        {
            if (controllerNamespaces.TryGetValue(controllerName, out var controllerNamespace))
            {
                var parts = controllerNamespace.Split('.');
                if (parts.Length > 2)
                {
                    var folderName = parts[2];

                    // Eğer folderName "Controllers" ise "anasayfa" döndür
                    if (folderName.Equals("Controllers", StringComparison.OrdinalIgnoreCase))
                    {
                        return "anasayfa";
                    }

                    return folderName; // Diğer durumlarda orijinal klasör adını döndür
                }
            }
            return "Anasayfa"; // Varsayılan olarak "anasayfa" döner
        }

        public Dictionary<string, string> GetControllerDisplayNames()
        {
            return new Dictionary<string, string>
            {
             
                { "KullaniciYonetimi", "KullanıcıYönetimi" }, 
               
                { "Role", "RolTanımları" },  
                { "UserRole", "KullanıcıRolleri" },
                { "Home", "Anasayfa" },
                { "Account", "Hesap" },
                { "Profile", "Profil" },
             { "Denetim", "Denetimler" },
                { "AracKayit", "AraçKayıt" },
                                { "Guzergah", "Güzergahlar" },

                { "CezaYonetimi", "Cezalar" },
                { "CezaYonetmelik", "CezaYönetmelik" },
                       { "Rapor", "Raporlar" }
        // Hikaye için açık kitap ikonu
                // Diğer controller adlarını burada ekleyin
            };
        }

        public Dictionary<string, string> GetActionDisplayNames()
        {
            return new Dictionary<string, string>
            {
                { "Index", "Listele" },
                { "Create", "Ekle" },
                { "Login", "Giriş" },
                { "Register", "Kayıt Ol" },
                { "AddUser", "Kullanıcı Ekle" },
                { "ListUsers", "Kullanıcıları Listele" },
                {"Assign", "Personele Birim Ata"},
                {"GetQuestion", "Soruları Getir"},

                {"CreateZamanCizelge", "Zaman Çizelgesi Ekle"},
                { "DenetimOlustur", "Ekle" },
                                { "CreateCeza", "Ekle" },
                                  { "Ihlal", "İhlal Cezaları" },
                                                                    { "Ihtar", "İhtar Cezaları" }




                // Diğer action adlarını burada ekleyin
            };
        }
    }
}
