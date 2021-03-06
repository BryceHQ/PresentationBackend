﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Json序列化设置
            var jsonSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;

            // 时间日期格式
            jsonSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonSettings.DateParseHandling = DateParseHandling.DateTime;
            jsonSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            //jsonSettings.MaxDepth = null;

            // 对 Json 序列化后的数据自动小写属性首字母 
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // 默认使用 ISO 时间格式序列化 Json 数据
            jsonSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            GlobalConfiguration.Configuration.Properties.TryAdd("uploadFolder", "upload");
        }
    }
}
