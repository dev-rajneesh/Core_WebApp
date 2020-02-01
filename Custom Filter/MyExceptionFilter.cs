using Core_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.Custom_Filter
{
    public class MyExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _metadataprovider;
        private readonly AppDbContext _appDbContext;
        public MyExceptionFilter(IModelMetadataProvider metadataprovider, AppDbContext appDbContext)
        {
            this._metadataprovider = metadataprovider;
            this._appDbContext = appDbContext; // Add to save error to DB
        }
        public override void OnException(ExceptionContext context)
        {
            //base.OnException(context);

            // Read exception message
            string message = context.Exception.Message;

            // Handle exception
            context.ExceptionHandled = true;

            // Goto view to display error messages
            var result = new ViewResult();

            // defining VeiwDataDictionary for Controller/action/errormessage
            var viewData = new ViewDataDictionary(_metadataprovider, context.ModelState);
            viewData["controller"] = context.RouteData.Values["controller"].ToString();
            viewData["action"] = context.RouteData.Values["action"].ToString();
            viewData["errormessage"] = message;

            // Set Viewname
            result.ViewName = "CustomError";

            // Set ViewData
            result.ViewData = viewData;

            // Setting result in HttpResponse
            context.Result = result;
        }
    }
}
