using ResistanceV2.DataContext;
using ResistanceV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace ResistanceV2.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        ResistanceDBContext _db = new ResistanceDBContext();
        public ActionResult Index()
        {
            
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string UserName = authCookie["userName"].ToString();
                UserModel matchUser = new UserModel(UserName);
                Session["User"] = matchUser; 
                return RedirectToAction("Feed", "Feed");
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult Index(UserModel User)
        {
                if (User.IsValid(User.UserName, User.Password))
                {
                    var userEntity = _db.User.ToList();

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                                1,
                                                                User.UserName,
                                                                DateTime.Now,
                                                                DateTime.Now.AddMinutes(10),
                                                                false,
                                                                null);

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie["userName"] = User.UserName;
                    this.Response.Cookies.Add(cookie);
                    return RedirectToAction("Feed", "Feed");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            //}
            return View(User);
        }

        public ActionResult Welcome()
        {
            UserModel User = (UserModel)TempData["User"];
            return View(User);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create( UserModel User1)
        {
           
            try
            {
                var checkUser = _db.User.Count(i => i.UserName == User1.UserName);

                if (checkUser==0 && ModelState.IsValid)
                {
                    _db.User.Add(User1);
                    _db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            catch
            {
                return View(User);
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

	}
}