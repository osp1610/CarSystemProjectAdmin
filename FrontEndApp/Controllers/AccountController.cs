using FrontEndApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEndApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        CarSystemEntities carSystemEntities= new CarSystemEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                UserData userData = new Models.UserData();
                userData.Username = user.UserName;
                userData.Email = user.UserEmail;
                userData.Contact = user.UserContact;
                userData.Password = user.UserPassword;
                userData.City = user.UserCity;
                LoginModel loginModel = new LoginModel();
                carSystemEntities.UserDatas.Add(userData);
                carSystemEntities.SaveChanges();

                return View(user);
            }


            else
            {

            }
            return View();
        }
        public ActionResult Login()
        {
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (carSystemEntities.UserDatas.Where(m => m.Email == loginModel.UserName && m.Password == loginModel.Password).FirstOrDefault() != null)
                {
                    UserData userData = new Models.UserData();
                    Session["Email"] = loginModel.UserName;
                    return RedirectToAction("UserHome", "Account");
                }
                else
                {

                }
            }
            return View();
        }

        public ActionResult UserHome()
        {
            return View();
        }

        public ActionResult SellCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SellCar(Car carDetails)
        {
            try
            {
                if (ModelState.IsValid==true)
                {

                    string fileName = Path.GetFileNameWithoutExtension(carDetails.ImageFile.FileName);
                    string extension = Path.GetExtension((carDetails.ImageFile.FileName));
                    HttpPostedFileBase postedfile = carDetails.ImageFile;
                    fileName = fileName + extension;
                    carDetails.CarImage = "~/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                    carDetails.ImageFile.SaveAs(fileName);

                    //LoginModel loginModel =new LoginModel();
                    //Car car = new Models.Car();
                    //car.CarNo=carDetails.CarNo;

                    //car.CarName = carDetails.CarName;
                    //car.CarModel=carDetails.CarModel;
                    //car.CarYear = carDetails.CarYear;
                    //car.CarType = carDetails.CarType;
                    //car.NoOfOwners= carDetails.NoOfOwners;
                    //car.CarVerified = false;
                    //car.CarSold = false;
                    //car.CarUid = loginModel.ID;
                    //car.City=carDetails.City;
                    var Uid = carSystemEntities.UserDatas.FirstOrDefault(s => s.UserId == 1);
                    carDetails.CarVerified = false;
                    carDetails.CarSold = false;
                    int u = Uid.UserId;
                    carDetails.CarUid=u;
                    carSystemEntities.Cars.Add(carDetails);
                    carSystemEntities.SaveChanges();

                }

                }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public ActionResult BuyCar()
        {
            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}