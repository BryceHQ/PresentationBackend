using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Web.Core;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AttachmentController : XController
    {
        /// <summary>
        /// 附件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                //上传实际中每次只上传一个文件
                HttpPostedFileBase file = files[files.AllKeys[0]];

                Stream stream = file.InputStream;

                string fileName = file.FileName;
                //ie下在通过form上传的时候的文件名会包含路径
                int index = fileName.LastIndexOf('\\');
                if (index != -1)
                {
                    fileName = fileName.Substring(index + 1);
                }
                var result = await AttachmentManager.Instance.UploadAttachment(fileName, this.CurrentUser.Id, stream);//
                if (!result.Succeeded)
                {
                    return Json(result.Errors);
                }
                return Json(result.Value.Url);
            }
            catch (Exception e)
            {
                return JsonError(e.Message);
            }
        }

        /// <summary>
        /// 附件下载
        /// </summary>
        /// <returns></returns>
        //public FileStreamResult DownLoad()
        //{
        //    int id = Helper.ToInt(Request["id"]);
        //    var retValue = AppHost.Current.Resolve<IFoundationService>().GetAttachmentById(id, true);
        //    if (!retValue.IsSuccess)
        //    {
        //        throw retValue.ToBusinessException();
        //    }
        //    return File(new FileStream(retValue.Value.FullPath, FileMode.Open), "application/octet-stream", retValue.Value.Name);
        //}

    }
}
