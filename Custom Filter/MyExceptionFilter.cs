using Core_WebApp.Models;
using Core_WebApp.Services;
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
        private readonly IRepository<ErrorLog, int> _errorRepository;
        public MyExceptionFilter(IModelMetadataProvider metadataprovider, AppDbContext appDbContext, IRepository<ErrorLog, int> errorRepository)
        {
            this._metadataprovider = metadataprovider;
            this._appDbContext = appDbContext; // Add to save error to DB
            this._errorRepository = errorRepository;
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

            // Log Error Details to database
            ErrorLog errorLog = new ErrorLog();
            errorLog.ErrorMessage = message;
            errorLog.ErrorDetails = "Controller: " + context.RouteData.Values["controller"].ToString() + " " +
                                    "Action: " + context.RouteData.Values["action"].ToString();
            errorLog.StackTrace = context.Exception.StackTrace;

            _errorRepository.CreateAsync(errorLog);
        }
    }
}
