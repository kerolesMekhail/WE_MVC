using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WE_Test.Helper
{
    public class BaseController : Controller
    {
        public List<SelectListItem> TypeDDL()
        {
            List<SelectListItem> View = new List<SelectListItem>();
           
            View.Add(new SelectListItem
            {
                Text = "موظفين جدد",
                Value = "1",
            });

            View.Add(new SelectListItem
            {
                Text = "موظفين تحت الاختبار",
                Value = "2",
            });
            
            View.Add(new SelectListItem
            {
                Text = "موظفين قائمين",
                Value = "3",
            });
            
            View.Add(new SelectListItem
            {
                Text = "موظفين محاليين للمعاش",
                Value = "4",
            });

            return View;
        }

    }
}
