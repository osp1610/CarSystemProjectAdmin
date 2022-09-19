using CarSystemProjectAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarSystemProjectAdmin.Controllers
{
    public class HomeController : Controller
    {

        CarSystemEntities carSystemEntities=new CarSystemEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AdminUser adminUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (carSystemEntities.AdminUsers.Where(m => m.AdminID == adminUser.AdminID && m.AdminPassword == adminUser.AdminPassword).FirstOrDefault() != null)
                    {
                        AdminUser admin = new Models.AdminUser();
                        Session["AdminID"] = adminUser.AdminID;
                        return RedirectToAction("AdminPanel", "Home");
                    }
                    else
                    {
                        // add exception condtion later
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        //In Admin Panel All the Users Data first shown default
        public ActionResult AdminPanel()
        {
            var res = carSystemEntities.UserDatas.ToList();
            return View(res);
        }
        [HttpGet]
        public ActionResult UserDelete(int id)
        {
           
            UserData userData = carSystemEntities.UserDatas.Single(x=>x.UserId==id);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedUserDelete(int id)
        {
            UserData userData = carSystemEntities.UserDatas.Find(id);
            carSystemEntities.UserDatas.Remove(userData);
            carSystemEntities.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        public ActionResult UserEdit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userData = carSystemEntities.UserDatas.Find(id);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit([Bind(Include = "UserId,Username,Email,Contact,Password,City")] UserData userData)
        {
            if (ModelState.IsValid)
            {
                carSystemEntities.Entry(userData).State = EntityState.Modified;
                carSystemEntities.SaveChanges();
                return RedirectToAction("AdminPanel");
            }
            return View(carSystemEntities);
        }
        


        //In this panel newly added cars will displayed to admin to verify and modify
        public ActionResult NewArrivals()
        {
            var res = carSystemEntities.Cars.Where(item => item.CarVerified == false).ToList();

           // var res = carSystemEntities.Cars.ToList();
            return View(res);
        }

        [HttpPost]
        public ActionResult Verify(Car car)
        {
            using (CarSystemEntities entities = new CarSystemEntities())
            {
                Car updatedCustomer = (from c in entities.Cars
                                            where c.CarNo == car.CarNo
                                            select c).FirstOrDefault();

                if (updatedCustomer != null)
                {
                    updatedCustomer.CarVerified = true;
                    entities.SaveChanges();
                    ViewBag.Message = "Customer record updated.";
                }
                else
                {
                    ViewBag.Message = "Customer not found.";
                }

                return View();
            }
        }

        //Here sold cars will be displyed
        public ActionResult SoldCars(Car car)
        {
            var res = carSystemEntities.Cars.Where(item => item.CarSold == true).ToList();

            //var cars = carSystemEntities.Cars;
            //var res = cars.ToList();
            //var res = carSystemEntities.Cars.ToList();

            return View(res);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


    }
}