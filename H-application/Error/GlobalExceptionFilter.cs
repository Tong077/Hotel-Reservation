using System;
using System.Data.Entity.Infrastructure; 
using System.Web.Mvc;

namespace H_application.Error
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            var innerMessage = ex.InnerException?.Message ?? ex.Message;

            var controller = filterContext.Controller as Controller;

            if (controller != null)
            {
                // Set TempData for Toastr notifications
                controller.TempData["toastr-type"] = "error";
                controller.TempData["toastr-message"] = ex is DbUpdateException
                    ? $"Database update error: {innerMessage}"
                    : $"Unexpected error: {innerMessage}";

                // Add error to ModelState
                controller.ViewData.ModelState.AddModelError("", innerMessage);

                // Return a view manually
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error", // You need to create Views/Shared/Error.cshtml
                    ViewData = controller.ViewData,
                    TempData = controller.TempData
                };

                filterContext.ExceptionHandled = true;
            }
        }
    }
}
