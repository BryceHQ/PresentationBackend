using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebMatrix.WebData;
using presentation.Business.Models;
using presentation.Business.Managers;
using presentation.Business;
using System.Data.Entity.Validation;

namespace presentation.Controllers
{
    public class PresentationController : Controller
    {
        [HttpPost]
        public ActionResult New(Presentation model)
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

        [HttpPost]
        public ActionResult Save()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }
            int id = Helper.ToInt(Request["id"]);
            string raw = Request["raw"].Trim();

            var model = PresentationManager.Instance.Get(id);
            model.Raw = raw;
            try
            {
                PresentationManager.Instance.Update(model);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Json(new { success = false, message = e.Message });
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
