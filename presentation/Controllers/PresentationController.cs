using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;
using presentation.Business.Models;
using presentation.Business.Managers;
using presentation.Business;

namespace presentation.Controllers
{
    public class PresentationController : Controller
    {
        [HttpPost]
        public ActionResult Save(Presentation model)
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var user = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
            model.UserId = user.UserId;
            try
            {
                PresentationManager.Instance.Add(model);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
            return Json(new { success = true });
        }

        public ActionResult Get()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            int id = Helper.ToInt(Request["id"]);
            var user = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
            var result = PresentationManager.Instance.FirstOrDefault(p => p.Id == id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPresentations()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var user = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
            var result = PresentationManager.Instance.Where(p => p.UserId == user.UserId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Json(ErrorCode errorCode)
        {
            return Json(new { success = false, code = errorCode.Code, message = errorCode.Description });
        }
    }
}
