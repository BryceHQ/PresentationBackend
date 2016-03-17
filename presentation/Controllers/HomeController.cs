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
                //var user = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
                //var last = PresentationManager.Instance.Where(a => a.UserId == user.UserId)
                //    .OrderBy(a => a.LastUpdateTime).First();
                //if (last != null)
                //{
                //    ViewBag.Raw = last;
                //}    
                ViewBag.returnUrl = "/home";
            }
            else
            {
                
            }
            return View();
        }
    }
}
