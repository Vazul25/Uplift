using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void LockUser(string userId)
        {
            var userToUpdate = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userToUpdate.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void UnlockUser(string userId)
        {
            var userToUpdate = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userToUpdate.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
