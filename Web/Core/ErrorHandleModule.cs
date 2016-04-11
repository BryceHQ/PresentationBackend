using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Controllers;

namespace Web.Core
{
    /// <summary>
    /// 处理未捕获的异常及4xx，5xx状态码的处理
    /// </summary>
    public class ErrorHandleModule : IHttpModule
    {

        public String ModuleName
        {
            get { return "ErrorHandleModule"; }
        }

        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(context_Error);
        }

        public void context_Error(object sender, EventArgs e)
        {
            //此处处理异常
            HttpApplication app = (HttpApplication)sender;

            Exception lastError = app.Server.GetLastError();

            if (lastError is HttpUnhandledException)
            {
                lastError = ((HttpUnhandledException)lastError).InnerException;
            }

            HttpException httpException = lastError is HttpException ? (HttpException)lastError : new HttpException(null, lastError);

            int httpStatusCode = httpException.GetHttpCode();

            int custErrorPageCode = 0;

            int internalErrorCode = 0;

            string internalErrorMessage = null;

            if (httpStatusCode > 0)
            {
                switch (httpStatusCode)
                {
                    // 默认提供以下错误码的自定义错误页支持
                    // HttpException 的 401 错误码转到登录页

                    case 404:
                        this.GetCustomErrorPages(app, httpException, httpStatusCode);
                        break;
                    case 403:
                    case 401:
                        //if (lastError is HttpException)
                        //{
                        //    app.Response.Clear();

                        //    app.Server.ClearError();

                        //    app.Response.TrySkipIisCustomErrors = true;

                        //    //重定向到登录页

                        //    return;
                        //}
                    case 405:
                    case 406:
                    case 412:
                        custErrorPageCode = httpStatusCode;
                        break;
                    default:
                        // 其它任何错误,都视为服务器内部错误，向用户展示 500 内部错误自定义错误页
                        custErrorPageCode = 500;

                        this.GetCustomErrorPages(app, httpException, 500);

                        break;
                }
            }
            else
            {
                httpStatusCode = 500;
                custErrorPageCode = 500;
            }


            // 普通的 http 异常不需要计算 internalErrorCode 和 internalErrorMessage，也不需要记录日志
            if (custErrorPageCode == 500)
            {
                //Exception logException = null;

                internalErrorCode = custErrorPageCode;

                internalErrorMessage = lastError.Message;

                // 通知诊断堆栈发生了需要记录日志的错误
                //DiagnosticsStack.Current.error = logException;
            }
            else
            {
                // 普通的 Http 错误是不需要记录日志的
                internalErrorCode = httpStatusCode;
                internalErrorMessage = httpException.GetHtmlErrorMessage();
            }

            app.Response.TrySkipIisCustomErrors = true;
            // 清空错误，将控制权交给 IIS
            app.Server.ClearError();

            // 集成管道模式时，对于非 500 的 http 异常使用 IIS 内置的错误页
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                // 返回原始 http 错误码
                app.Response.StatusCode = httpStatusCode;


                if (custErrorPageCode == 500 || custErrorPageCode == 401)
                {
                    // 通过标头将错误发送给客户端,Json请求时直接从这里拿到错误消息
                    app.Response.AddHeader("server-error", 
                        Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(
                                String.Format("{{\"code\":{0}, \"message\":\"{1}\"}}", internalErrorCode, internalErrorMessage))
                        )
                    );
                }
            }
            // 经典模式使用 asp.net 原始自定义错误页机制
            else
            {
                // 返回原始 http 错误码
                app.Response.StatusCode = httpStatusCode;

                if (custErrorPageCode == 500 || custErrorPageCode == 401)
                {
                    // 通过标头将错误发送给客户端
                    app.Response.AddHeader("server-error", Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            String.Format("{{\"code\":{0}, \"message\":\"{1}\"}}", internalErrorCode, internalErrorMessage))
                        )
                    );
                }

                // Certain versions of IIS will sometimes use their own error page when
                // they detect a server error. Setting this property indicates that we
                // want it to try to render ASP.NET MVC's error page instead.

                app.Response.End();
            }
        }

        private void GetCustomErrorPages(HttpApplication app, HttpException ex, int statusCode)
        {
            app.Response.Clear();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            switch (statusCode)
            {
                case 404:
                    routeData.Values.Add("action", "NotFound");
                    break;
                case 500:
                    routeData.Values.Add("action", "Index");
                    break;
                default:
                    break;
            }
            routeData.Values.Add("error", ex);

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(
                new RequestContext(
                    new HttpContextWrapper(app.Context), routeData)
            );
        }
    }
}