using Microsoft.AspNetCore.Mvc;
using Portal.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Portal.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly MenuHelper _menuHelper;

        public MenuViewComponent(MenuHelper menuHelper)
        {
            _menuHelper = menuHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = _menuHelper.GetAccessibleMenus();

            var groupedMenus = menus
                .GroupBy(m => m.ControllerFolder)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(m => m.DisplayController)
                          .ToDictionary(
                              g => g.Key,
                              g => g.OrderBy(m => m.DisplayAction).ToList()
                          )
                );

            return View(groupedMenus);
        }
    }
}


// using Microsoft.AspNetCore.Mvc;
// using Portal.Helpers;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Collections.Generic;

// namespace Portal.ViewComponents
// {
//     public class MenuViewComponent : ViewComponent
//     {
//         private readonly MenuHelper _menuHelper;

//         public MenuViewComponent(MenuHelper menuHelper)
//         {
//             _menuHelper = menuHelper;
//         }

//         public async Task<IViewComponentResult> InvokeAsync()
//         {
//             var menus = _menuHelper.GetAccessibleMenus();

//             var groupedMenus = menus
//                 .GroupBy(m => m.DisplayController)
//                 .OrderBy(g => g.Key)
//                 .ToDictionary(
//                     g => g.Key,
//                     g => g.OrderBy(m => m.DisplayAction).ToList()
//                 );

//             return View(groupedMenus);
//         }
//     }
// }
