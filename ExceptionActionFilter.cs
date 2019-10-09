using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CoreAPICourse
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {
                //web api exception
                context.HttpContext.Response.StatusCode =(int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult("Web API Controller exception "+ context.Exception.Message + "--" + context.HttpContext.Response.StatusCode);


            }
            else if (controllerType.IsSubclassOf(controller))
            {
                //mvc controller exception
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult("MVC Controller Exception: " + context.Exception.Message + "--" + context.HttpContext.Response.StatusCode);


            }


            base.OnException(context);
        }

    }
}
