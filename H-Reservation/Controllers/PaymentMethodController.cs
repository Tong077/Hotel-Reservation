using H_application.DTOs.Payment;
using H_application.DTOs.PaymentMethodDto;
using H_application.Service;
using H_Domain.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodService _service;
        public PaymentMethodController(IPaymentMethodService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllPaymentMethods();
            return View("Index", result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(PaymentMethodDtoCreate paymentmethod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", paymentmethod);
                }
                var result = await _service.CreatePaymentMethodAsync(paymentmethod);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Create", paymentmethod);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", paymentmethod);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetPaymentMethodById(id, default);
            return View("Edit", result);
        }
        public async Task<IActionResult> Update(PaymentMethodDtoUpdate paymentmethod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", paymentmethod);
                }
                var result = await _service.UpdatePaymentMethodAsync(paymentmethod);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Edit", paymentmethod);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", paymentmethod);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.GetPaymentMethodById(id, default);
            return View("Delete", result);
        }
        public async Task<IActionResult> Destroy(PaymentMethodDtoUpdate paymentmethod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete", paymentmethod);
                }
                var result = await _service.DeletePaymentMethodAsync(paymentmethod);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Delete", paymentmethod);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Delete", paymentmethod);
            }
        }
    }
}
