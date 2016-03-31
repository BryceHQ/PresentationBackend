using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core;
using Business;
using Business.Models;
using Business.Managers;
using System.IO;

namespace presentation.Controllers
{
    public class AttachmentController : XController
    {
        /// <summary>
        /// 附件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                var description = Helper.Trim(Request["description"]);

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
                var url = AttachmentManager.Instance.UploadAttachment(fileName, description, stream);//
                
                //Attachment result = retValue.Value;
                return Json(url);
            }
            catch (Exception e)
            {
                return Json(new ErrorCode(e.Message));
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
