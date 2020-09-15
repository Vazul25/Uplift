using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }
            frequency = _unitOfWork.Frequency.GetFirstOrDefault(i => i.Id == id);
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    _unitOfWork.Frequency.Add(frequency);
                }
                else
                {
                    _unitOfWork.Frequency.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }
        #region PUBLIC API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
           var frequencyToDelete = _unitOfWork.Frequency.Get(id);
            if(frequencyToDelete == null)
            {
                return NotFound(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.Frequency.Remove(frequencyToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = $"Frequency {frequencyToDelete.Name} is deleted successfully" });
        }
        
         

        #endregion

    }
}