using FrontEndApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndApp.Controllers
{
    public class HomeController : Controller
    {
        CarSystemEntities carSystemEntities=new CarSystemEntities();
        string carno,carname;
        public ActionResult Index()
        {
           
            var res = carSystemEntities.Cars.ToList();
            foreach(var item in res)
            {
                carno = item.CarNo;
                carname = item.CarName;
                
            return View(item);
            }

            return View();
        }

      
    }
}