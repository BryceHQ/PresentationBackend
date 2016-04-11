using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : XController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.User = this.CurrentUser;
                ViewBag.returnUrl = "/home";
            }
            else
            {

            }
            return View();
        }

        //
        // GET: /Error/PageNotFound
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}