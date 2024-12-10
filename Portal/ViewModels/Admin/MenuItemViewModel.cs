using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.ViewModels.Admin
{
    public class MenuItemViewModel
    {
         public string OriginalController { get; set; }
    public string OriginalAction { get; set; }
    public string DisplayController { get; set; }
    public string DisplayAction { get; set; }
    public string ControllerFolder { get; set; } // Klasör ismi ekleme

        
    }
}