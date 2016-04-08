using System;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using System.Web.Http;
using System.Text;

using Web.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Web.Controllers
{
    public abstract class XController : Controller
    {
        private ApplicationUser _currentUser;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var userId = User.Identity.GetUserId();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        _currentUser = manager.FindById(userId);
                    }
                }
                return _currentUser;
            }
        }
        
        /// <summary>
        /// 用户重新登录后重置CurrentUser
        /// </summary>
        protected void ResetUser()
        {
            _currentUser = null; 
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


        #region 拓展 Json 错误处理
        /// <summary>
        /// ModelState中的错误信息以Json形式返回
        /// </summary>
        /// <returns></returns>
        protected JsonResult Json()
        {
            return Json(JsonRequestBehavior.DenyGet);
        }
        protected JsonResult Json(JsonRequestBehavior behavior)
        {
            var errors = new List<ErrorCode>();
            foreach (var state in this.ModelState)
            {
                if (state.Value != null)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(new ErrorCode(error.ErrorMessage));
                    }
                }
            }
            return Json(errors, behavior);
        }

        protected JsonResult Json(ErrorCode errorCode)
        {
            return Json(errorCode, JsonRequestBehavior.DenyGet);
        }
        protected JsonResult Json(ErrorCode errorCode, JsonRequestBehavior behavior)
        {
            return Json(new { Success = false, Code = errorCode.Code, Message = errorCode.Description }, behavior);
        }

        protected JsonResult Json(IEnumerable<ErrorCode> errorCodes)
        {
            return Json(errorCodes, JsonRequestBehavior.DenyGet);
        }
        protected JsonResult Json(IEnumerable<ErrorCode> errorCodes, JsonRequestBehavior behavior)
        {
            return Json(new { Success = false, Message = errorCodes}, behavior);
        }


        protected JsonResult JsonError(string error)
        {
            return JsonError(error, JsonRequestBehavior.DenyGet);
        }
        protected JsonResult JsonError(string error, JsonRequestBehavior behavior)
        {
            return Json(new { Success = false, Message = error}, behavior);
        }
        #endregion


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

        /// <summary>
        /// 拓展的JsonResult， 以实现Json属性名转为小驼峰，日期时间格式化等功能
        /// </summary>
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
                    response.Write(
                        JsonConvert.SerializeObject(
                            Data,
                            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
                        )
                    );
                }
            }
        }

    }
}
