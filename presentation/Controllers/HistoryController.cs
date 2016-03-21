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
    public class HistoryController : XController
    {
        /// <summary>
        /// 恢复某版本历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Recovery()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }
            
            int id = Helper.ToInt(Request["id"]);
            var history = HistoryManager.Instance.Get(id);
            if (history == null)
            {
                return Json(new ErrorCode(string.Format("id 为 {0} 历史记录不存在", id)));
            }
            var presentation = PresentationManager.Instance.Get(history.PresentationId);
            if (presentation == null)
            {
                return Json(new ErrorCode(string.Format("id 为 {0} 文件不存在", history.PresentationId)));
            }

            var returnValue = PresentationManager.Instance.SaveWithHistory(history.Raw, presentation);
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
            var result = HistoryManager.Instance.Get(id);
            if (result == null)
            {
                return Json(new ErrorCode("当前文件不存在"));
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult All()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }
            int presentationId = Helper.ToInt(Request["id"]);

            var result = HistoryManager.Instance.Where(h => h.PresentationId == presentationId)
                .OrderByDescending(h => h.LastUpdateTime);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
