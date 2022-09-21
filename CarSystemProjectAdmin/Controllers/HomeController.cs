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
        [HttpGet]
        public ActionResult AdminPanel(string SearchBy, string SearchVal)
        {
            try
            {
                var res = carSystemEntities.UserDatas.ToList();
                if (res.Count == 0)
                {
                    TempData["InfoMessage"] = "User Data Invalid";
                }
                else
                {
                    if (string.IsNullOrEmpty(SearchVal))
                    {
                        TempData["InfoMessage"] = "User Data Invalid";
                        return View(res);
                    }
                    else if(SearchBy.ToLower()== "username")
                    {
                        var SearchByUsername = res.Where(p => p.Username.ToLower().Contains(SearchVal.ToLower()));
                        return View(SearchByUsername);
                    }
                    else if (SearchBy == "UserId")
                    {
                        var SearchByUsername = res.Where(p => p.City.ToLower().Contains(SearchVal.ToLower()));
                        return View(SearchByUsername);
                    }
                }

                return View(res);
            }
            catch (Exception e)
            {
                TempData["ErrorMsg"]=e.Message;
                return View();
            }
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

        public ActionResult VerifyCars(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carSystemEntities.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyCars([Bind(Include = "CarNo,CarImage,CarName,CarModel,CarYear,CarType,NoOfOwners,CarVerified,CarSold,CarUid,City")] Car car)
        {
            if (ModelState.IsValid)
            {
                carSystemEntities.Entry(car).State = EntityState.Modified;
                carSystemEntities.SaveChanges();
                return RedirectToAction("NewArrivals");
            }
            return View(car);
        }
        public ActionResult DeleteCars(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carSystemEntities.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("DeleteCars")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = carSystemEntities.Cars.Find(id);
            carSystemEntities.Cars.Remove(car);
            carSystemEntities.SaveChanges();
            return RedirectToAction("NewArrivals");
        }


        public ActionResult EditCars(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carSystemEntities.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCars([Bind(Include = "CarNo,CarImage,CarName,CarModel,CarYear,CarType,NoOfOwners,CarVerified,CarSold,CarUid,City")] Car car)
        {
            if (ModelState.IsValid)
            {
                carSystemEntities.Entry(car).State = EntityState.Modified;
                carSystemEntities.SaveChanges();
                return RedirectToAction("NewArrivals");
            }
            return View(car);
        }

        public ActionResult AddCars()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCars([Bind(Include = "CarNo,CarImage,CarName,CarModel,CarYear,CarType,NoOfOwners,CarVerified,CarSold,CarUid,City")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.CarVerified = true;
                car.CarSold = false;
                carSystemEntities.Cars.Add(car);
                carSystemEntities.SaveChanges();
                return RedirectToAction("NewArrivals");
            }

            return View(car);
        }





        //Here sold cars will be displyed
        public ActionResult SoldCars(Car car)
        {
            var res = carSystemEntities.Cars.Where(item => item.CarSold == false && item.CarVerified==true).ToList();
            return View(res);
        }

        public ActionResult SoldCar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = carSystemEntities.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SoldCar([Bind(Include = "CarNo,CarImage,CarName,CarModel,CarYear,CarType,NoOfOwners,CarVerified,CarSold,CarUid,City")] Car car)
        {
            if (ModelState.IsValid)
            {
                carSystemEntities.Entry(car).State = EntityState.Modified;
                carSystemEntities.SaveChanges();
                return RedirectToAction("SoldCars");
            }
            return View(car);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


    }
}