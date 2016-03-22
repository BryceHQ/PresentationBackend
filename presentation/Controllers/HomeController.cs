using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;
using Business.Managers;

namespace presentation.Controllers
{
    public class HomeController : XController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = AccountManager.Instance.FirstOrDefault(a => a.Name == User.Identity.Name);

                ViewBag.User = user;
                ViewBag.returnUrl = "/home";
            }
            else
            {
                
            }
            return View();
        }
    }
}
