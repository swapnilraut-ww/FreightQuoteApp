using FreightQuote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FreightQuote.Web.Controllers
{
    public class BaseController : Controller
    {
        private FreightQuoteEntities _db;
        public FreightQuoteEntities db
        {
            get
            {
                if (_db == null)
                {
                    _db = new FreightQuoteEntities();
                }
                return _db;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (!(String.Equals(controllerName, "Account", StringComparison.OrdinalIgnoreCase) && String.Equals(actionName, "Login", StringComparison.OrdinalIgnoreCase)))
            {
                if (FreightSession.Current.User == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "Login"
                    }));
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}