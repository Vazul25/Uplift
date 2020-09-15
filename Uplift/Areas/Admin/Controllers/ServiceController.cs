using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
   

    [Area("Admin")]
    [Authorize]

    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;



        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviroment = hostEnviroment;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceViewModel serviceViewModel = new ServiceViewModel()
            {
                Service = new Models.Service(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropdown()
            };
            if (id != null)
            {
                serviceViewModel.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(serviceViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceViewModel serviceViewModel)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnviroment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (serviceViewModel.Service.Id == 0)
                {
                     
                    serviceViewModel.Service.ImageUrl = ServiceUtility.CreateServiceImage(webRootPath,files[0]);
                    _unitOfWork.Service.Add(serviceViewModel.Service);

                }
                else
                {
                    //Edit service
                    var serviceFromDb = _unitOfWork.Service.Get(serviceViewModel.Service.Id);
                    if (files.Count > 0)
                    {
                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        serviceViewModel.Service.ImageUrl = ServiceUtility.CreateServiceImage(webRootPath, files[0]);
                    }
                    else
                    {
                        serviceViewModel.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }
                    _unitOfWork.Service.Update(serviceViewModel.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                serviceViewModel.CategoryList = _unitOfWork.Category.GetCategoryListForDropdown();
                serviceViewModel.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(serviceViewModel);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency") });
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            string webRootPath = _hostEnviroment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
          
            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion
    }
    public class ServiceUtility
    {
        internal static string CreateServiceImage(string webRootPath, IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(webRootPath, @"images\services");
            var extension = Path.GetExtension(file.FileName);

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            return @"\images\services\" + fileName + extension;
        }
    }
}
