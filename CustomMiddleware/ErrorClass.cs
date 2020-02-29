using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.CustomMiddleware
{
    public class ErrorClass
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    // This is Custom Middleware

    public class ErrorMiddleware
    {
        /// We need to inject the "RequestDelegate" class.
        /// This will be used to process httprequest
        /// 
        RequestDelegate requestDelegate;
        public ErrorMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        /// We need to add the InvokeAsync() method. 
        /// We need this method so that RequestDelegate can process the HttpRequest
        /// 
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch(Exception ex)
            {
                await HandleErrorAsync(httpContext, ex);
            }
        }

        /// Method to handle Exceptions occures while Request processing
        /// 
        public static Task HandleErrorAsync(HttpContext httpContext, Exception exception)
        {
            // send the status code based on the error occured
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            // Get the thrown error
            string errorMessage = exception.Message;

            // Create the response
            string ResponseMessage = JsonConvert.SerializeObject(new ErrorClass()
            {
                ErrorCode = httpContext.Response.StatusCode,
                ErrorMessage = errorMessage
            });

            // Get the response message
            return httpContext.Response.WriteAsync(ResponseMessage);
        }        
    }

    /// The class that registers the middleware
    /// 
    public static class CustomErrorExtensionsMiddleware
    {
        /// The following method will use ErrorMiddleware class
        /// 
        public static void UseCustomExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorMiddleware>();
        }
    }
}
