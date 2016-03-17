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
            return View("~/Views/Home/Index.cshtml");;
        }

        public ActionResult SignUp()
        {
            return View("~/Views/Home/Index.cshtml"); ;
        }
    }
}
