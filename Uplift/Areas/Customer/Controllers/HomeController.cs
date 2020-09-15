using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var homeViewModell = new HomeViewModell() { 
            CategoryList = _unitOfWork.Category.GetAll(),
            ServiceList = _unitOfWork.Service.GetAll(includeProperties:"Frequency"),
            };
            return View(homeViewModell);
        }
        public IActionResult Details(int id)
        {
            var serviceFromDb = _unitOfWork.Service.GetFirstOrDefault(includeProperties: "Frequency,Category", filter: c => c.Id == id);
            return View(serviceFromDb);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);                
            }
            if (!sessionList.Contains(serviceId))
            {
                sessionList.Add(serviceId);
            }
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
