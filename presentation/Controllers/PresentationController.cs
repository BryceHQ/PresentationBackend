using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core;
using Business;
using Business.Models;
using Business.Managers;
using System.Data.Entity.Validation;

namespace presentation.Controllers
{
    public class PresentationController : XController
    {
        string defaultName = "未命名";
        string defaultRaw = "[{\"transition\":\"zoom\",\"content\":\"# 请输入标题\", \"key\":\"1\"}]";

        [HttpPost]
        public ActionResult Add()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            string name = Helper.Trim(Request["name"]);
            int folderId = Helper.ToInt(Request["folderId"]);

            var model = new Presentation()
            {
                Name = defaultName,
                UserId = this.CurrentUser.UserId,
                Raw = defaultRaw
            };

            try
            {
                var result = PresentationManager.Instance.Add(model);
                return Json(result.Id);
            }
            catch (Exception e)
            {
                return Json(new ErrorCode(e.Message));
            }
        }

        [HttpPost]
        public ActionResult Save()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            int id = Helper.ToInt(Request["id"]);
            string raw = Helper.Trim(Request["raw"]);
            string name = Helper.Trim(Request["name"]);

            var model = PresentationManager.Instance.Get(id);
            if (model == null)
            {
                return Json(new ErrorCode("当前文件不存在"));
            }
            if (!string.IsNullOrWhiteSpace(raw)) 
            {
                model.Raw = raw;
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                model.Name = name;
            }
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
                return Json(new ErrorCode(e.Message));
            }
            catch (Exception e)
            {
                return Json(new ErrorCode(e.Message));
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

        public ActionResult All()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var result = PresentationManager.Instance.Where(p => p.UserId == this.CurrentUser.UserId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Recent()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var user = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
            var result = PresentationManager.Instance.Where(p => p.UserId == user.UserId);

            return Json(result , JsonRequestBehavior.AllowGet);
        }
    }
}
