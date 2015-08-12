using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FreightQuote.Web.Controllers
{
    public static class ControllerExtensions
    {
        public static T DeserializeObject<T>(this Controller controller, string key) where T : class
        {
            var value = controller.HttpContext.Request.QueryString.Get(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<T>(value);
        }
    }
}