using FreightQuote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreightQuote.Web.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            User objuser = db.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password).SingleOrDefault();
            if (objuser != null)
            {
                FreightSession.Current.User = user;
                Session["UserName"] = user.UserName;
                return RedirectToAction("List", "Quote");
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(user);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}