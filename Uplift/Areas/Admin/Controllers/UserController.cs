﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Admin)]

    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_unitOfWork.ApplicationUser.GetAll(u=> u.Id!=claims.Value));
        }
        public IActionResult Lock(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _unitOfWork.ApplicationUser.LockUser(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _unitOfWork.ApplicationUser.UnlockUser(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
