using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;

namespace presentation.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult SignIn()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
