using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;
        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Service service)
        {
            var serviceToUpdate = _db.Service.FirstOrDefault(s => service.Id == s.Id);
            serviceToUpdate.Name = service.Name;
            serviceToUpdate.Price = service.Price;
            serviceToUpdate.LongDesc = service.LongDesc;
            serviceToUpdate.ImageUrl = service.ImageUrl;
            serviceToUpdate.FrequencyId = service.FrequencyId;
            serviceToUpdate.CategoryId = service.CategoryId;
            
             
            _db.SaveChanges();
        }
    }
}
