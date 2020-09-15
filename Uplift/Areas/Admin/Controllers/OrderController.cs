using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            OrderViewModel orderVM = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(filter: o => o.OrderHeaderId == id)
            };
            return View(orderVM);
        }
        public IActionResult Approve(int id)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusApproved);
            return View(nameof(Index));
        }
        public IActionResult Reject(int id)
        {
            var orderFromDb = _unitOfWork.OrderHeader.Get(id);
            if (orderFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderHeader.ChangeOrderStatus(id, SD.StatusRejected);
            return View(nameof(Index));
        }
        #region API Calls

        public IActionResult GetAllOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll() });
        }
        public IActionResult GetApprovedOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusApproved) });
        }
        public IActionResult GetPendingOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusSubmitted) });
        }
        public IActionResult GetRejectedOrders()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll(filter: o => o.Status == SD.StatusRejected) });
        }
        #endregion
    }
}
