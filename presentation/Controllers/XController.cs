using System;
using System.Web;
using System.Web.Mvc;

using Core;
using Newtonsoft.Json;
using System.Web.Http;
using System.Text;
using Business.Models;
using Business.Managers;

namespace presentation.Controllers
{
    public abstract class XController : Controller
    {
        private UserProfile currentUser;

        public UserProfile CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = AccountManager.Instance.FirstOrDefault(a => a.UserName == User.Identity.Name);
                }
                return currentUser;
            }
        }
        /// <summary>
        /// 获取一个值，该值指示当前请求是否是 GET 请求。
        /// </summary>
        public bool IsGetRequest
        {
            get
            {
                return this.Request.HttpMethod == "GET";
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前请求是否是 POST 请求。
        /// </summary>
        /// <returns></returns>
        public bool IsPostRequest()
        {
            return this.Request.HttpMethod == "POST";
        }

        /// <summary>
        /// 获取一个值，该值指示当前请求是否是 HEAD 请求。
        /// </summary>
        /// <returns></returns>
        public bool IsHeadRequest()
        {
            return this.Request.HttpMethod == "HEAD";
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new XJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected JsonResult Json(ErrorCode errorCode)
        {
            return Json(new { success = false, code = errorCode.Code, message = errorCode.Description });
        }

        private class XJsonResult : JsonResult
        {
            public XJsonResult()
                : base()
            {
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    // 调用 JsonResult 内部的机制抛出异常
                    base.ExecuteResult(context);
                    return;
                }

                HttpResponseBase response = context.HttpContext.Response;

                if (!String.IsNullOrEmpty(ContentType))
                {
                    response.ContentType = ContentType;
                }
                else
                {
                    response.ContentType = "application/json";
                }
                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }
                if (Data != null)
                {
                    // 使用 Json.net 的序列化机制代替 .net 内置机制

                    //JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //if (MaxJsonLength.HasValue)
                    //{
                    //    serializer.MaxJsonLength = MaxJsonLength.Value;
                    //}
                    //if (RecursionLimit.HasValue)
                    //{
                    //    serializer.RecursionLimit = RecursionLimit.Value;
                    //}
                    //response.Write(serializer.Serialize(Data));

                    response.Write(JsonConvert.SerializeObject(
                            Data,
                            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
                        ));
                }
            }
        }

    }

}
