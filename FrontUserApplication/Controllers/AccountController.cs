using FrontUserApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontUserApplication.Controllers
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
                carSystemEntities.UserDatas.Add(userData);
                carSystemEntities.SaveChanges();

                return View(user);
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}