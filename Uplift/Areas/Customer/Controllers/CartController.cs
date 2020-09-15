using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartViewModel CartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ServiceList = new List<Service>()
            };
        }
        public IActionResult Index()
        {
            var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            if (sessionList != null)
            {
                var services = _unitOfWork.Service.GetAll(s => sessionList.Contains(s.Id),includeProperties: "Frequency,Category").ToList();
                CartVM.ServiceList = CartVM.ServiceList.Concat(services).ToList();
            }
            return View(CartVM);
        }

        public IActionResult Remove(int serviceId)
        {
            var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart,sessionList);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            if (sessionList != null)
            {
                var services = _unitOfWork.Service.GetAll(s => sessionList.Contains(s.Id), includeProperties: "Frequency,Category").ToList();
                CartVM.ServiceList = CartVM.ServiceList.Concat(services).ToList();
            }
            return View(CartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            if (sessionList != null)
            {
                var services = _unitOfWork.Service.GetAll(s => sessionList.Contains(s.Id), includeProperties: "Frequency,Category").ToList();
                CartVM.ServiceList = new List<Service>();
                CartVM.ServiceList = CartVM.ServiceList.Concat(services).ToList();
            }
            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = SD.StatusSubmitted;
                CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;
                 _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
                _unitOfWork.Save();
                foreach(var item in CartVM.ServiceList)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        ServiceId = item.Id,
                        OrderHeaderId = CartVM.OrderHeader.Id,
                        ServiceName = item.Name,
                        Price = item.Price
                    };
                    _unitOfWork.OrderDetails.Add(orderDetails);
                   
                }
                _unitOfWork.Save();
                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction("OrderConfirmation", "Cart", new { id = CartVM.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {

            return View(id);
        }
    }
}
