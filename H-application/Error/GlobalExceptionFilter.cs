//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Web.Mvc;
//namespace H_Reservation.Error
//{
//    public class GlobalExceptionFilter : IExceptionFilter
//    {
//        public void OnException(ExceptionContext context)
//        {
//            var ex = context.Exception;
//            var message = ex.InnerException?.Message ?? ex.Message;

//            if (context.Controller is Controller controller)
//            {
//                controller.TempData["toastr-type"] = "error";
//                controller.TempData["toastr-message"] = message;

//                context.Result = new ViewResult
//                {
//                    ViewName = "Error",
//                    ViewData = controller.ViewData,
//                    TempData = controller.TempData
//                };

//                context.ExceptionHandled = true;
//                return;
//            }
//        }
//    }
//}
