using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Core;

namespace Web.Controllers
{
    public class PresentationController : XController
    {
        private string _defaultName = "未命名";
        private string _defaultDuang = "gradient left";
        private string _defaultRaw = "[{\"transition\":\"fade\",\"content\":\"# 请输入标题\", \"key\":\"1\"}]";

        [HttpPost]
        public async Task<ActionResult> Add()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            string name = Helper.Trim(Request["name"]);
            int folderId = Helper.ToInt(Request["folderId"]);

            var model = new Presentation()
            {
                Name = _defaultName,
                UserId = this.CurrentUser.Id,
                Raw = _defaultRaw,
                FolderId = folderId,
                Duang = _defaultDuang
            };

            var result = await PresentationManager.Instance.Add(model);
            if (!result.Succeeded)
            {
                return Json(result.Errors);
            }
            return Json(result.Value.Id);
        }

        [HttpPost]
        public async Task<ActionResult> Save()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            int id = Helper.ToInt(Request["id"]);
            string raw = Helper.Trim(Request["raw"]);
            string name = Helper.Trim(Request["name"]);
            string background = Helper.Trim(Request["background"]);

            var model = await PresentationManager.Instance.Find(id);
            if (model == null)
            {
                return Json(string.Format("id 为 {0} 文件不存在", id));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                model.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(background))
            {
                model.Background = background;
            }

            IAsyncResult<Presentation> returnValue;
            if (string.IsNullOrWhiteSpace(raw))
            {
                returnValue = await PresentationManager.Instance.Update(model);
            }
            else
            {
                returnValue = await PresentationManager.Instance.SaveWithHistory(raw, model);
            }
            if (!returnValue.Succeeded)
            {
                return Json(returnValue.Errors);
            }
            return Json(string.Empty);
        }

        [Authorize]
        public async Task<ActionResult> Get()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated, JsonRequestBehavior.AllowGet);
            }


            int id = Helper.ToInt(Request["id"]);
            var result = await PresentationManager.Instance.FirstOrDefault(p => p.Id == id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> All()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated, JsonRequestBehavior.AllowGet);
            }

            var result = await PresentationManager.Instance.FindByUserId(this.CurrentUser.Id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Recent()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated, JsonRequestBehavior.AllowGet);
            }
            var result = await PresentationManager.Instance.FindByUserId(this.CurrentUser.Id);

            return Json(result.OrderByDescending(p => p.LastUpdateTime).Take(5), JsonRequestBehavior.AllowGet);
        }
    }
}
