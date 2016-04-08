using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Core;

namespace Web.Controllers
{
    public class HistoryController : XController
    {
        /// <summary>
        /// 恢复某版本历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Recovery()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated);
            }

            int id = Helper.ToInt(Request["id"]);
            var history = await HistoryManager.Instance.Find(id);
            if (history == null)
            {
                return JsonError(string.Format("id 为 {0} 历史记录不存在", id));
            }
            var presentation = await PresentationManager.Instance.Find(history.PresentationId);
            if (presentation == null)
            {
                return JsonError(string.Format("id 为 {0} 文件不存在", history.PresentationId));
            }

            var returnValue = await PresentationManager.Instance.SaveWithHistory(history.Raw, presentation);
            if (!returnValue.Succeeded)
            {
                return Json(returnValue.Errors);
            }
            return Json(string.Empty);
        }


        public async Task<ActionResult> Get()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated, JsonRequestBehavior.AllowGet);
            }

            int id = Helper.ToInt(Request["id"]);
            var result = await HistoryManager.Instance.Find(id);
            if (result == null)
            {
                return JsonError("当前文件不存在");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> All()
        {
            if (!Request.IsAuthenticated)
            {
                return Json(NestedErrorCodes.NotAuthenticated, JsonRequestBehavior.AllowGet);
            }
            int presentationId = Helper.ToInt(Request["id"]);

            var result = await HistoryManager.Instance.FindByPresentationId(presentationId);

            return Json(result.OrderByDescending(h => h.LastUpdateTime), JsonRequestBehavior.AllowGet);
        }
    }
}
