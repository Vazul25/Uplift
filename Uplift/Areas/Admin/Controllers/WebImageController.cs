using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class WebImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public WebImageController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            WebImage imageObj = new WebImage();
            if (id == null)
            {
                return View(imageObj);
            }
            imageObj = _unitOfWork.WebImage.GetFirstOrDefault(wi=>wi.Id==id);
            if(imageObj == null)
            {
                return NotFound();
            }
            return View(imageObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id,WebImage imageObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream()) 
                    {
                        using(var ms1=new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    imageObj.Picture = p1;
                }
                if(imageObj.Id == 0)
                {
                    _unitOfWork.WebImage.Add(imageObj);
                    //_db.WebImage.Add(imageObj);
                }
                else
                {
                    var imageFromDb = _unitOfWork.WebImage.GetFirstOrDefault(c => c.Id == id);
                    if (imageFromDb == null)
                    {
                        return NotFound();
                    }
                    imageFromDb.Name = imageObj.Name;
                    if (files.Count > 0)
                    {
                        imageFromDb.Picture = imageObj.Picture;
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(imageObj);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //return Json(new { data = _db.WebImage.ToList() });
            return Json(new { data = _unitOfWork.WebImage.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.WebImage.GetFirstOrDefault(wi=>wi.Id==id);
            //var objFromDb2 = _db.WebImage.Find(id);
            if (objFromDb == null)
            {
                return NotFound(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.WebImage.Remove(objFromDb);
            //_db.WebImage.Remove(objFromDb);
            _unitOfWork.Save();
            //_db.SaveChanges();
            return Json(new { success = true, message = $"Category {objFromDb.Name} is deleted successfully" });
        }
        #endregion
    }
}