using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core;
using Business;
using Business.Models;
using Business.Managers;

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
                UserId = this.CurrentUser.Id,
                Raw = defaultRaw,
                FolderId = folderId
            };

            var returnValue = PresentationManager.Instance.Add(model);
            if (!returnValue.Successed)
            {
                return Json(returnValue.ErrorCode);
            }
            return Json(returnValue.Value.Id);
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

            if (!string.IsNullOrWhiteSpace(name))
            {
                model.Name = name;
            }

            ReturnValue<Presentation> returnValue;
            if (string.IsNullOrWhiteSpace(raw))
            {
                returnValue = PresentationManager.Instance.Update(model);
            }
            else
            {
                returnValue = PresentationManager.Instance.SaveWithHistory(raw, model);
            }
            if (!returnValue.Successed)
            {
                return Json(returnValue.ErrorCode);
            }
            return Json(string.Empty);
        }

        public ActionResult Get()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            int id = Helper.ToInt(Request["id"]);
            var user = AccountManager.Instance.FirstOrDefault(a => a.Name == User.Identity.Name);
            var result = PresentationManager.Instance.FirstOrDefault(p => p.Id == id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult All()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var result = PresentationManager.Instance.Where(p => p.UserId == this.CurrentUser.Id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Recent()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            var user = AccountManager.Instance.FirstOrDefault(a => a.Name == User.Identity.Name);
            var result = PresentationManager.Instance.Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.LastUpdateTime)
                .Take(5);

            return Json(result , JsonRequestBehavior.AllowGet);
        }
    }
}
