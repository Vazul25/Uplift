using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;
        public FrequencyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _db.Frequency.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            var oldFrequency = _db.Frequency.FirstOrDefault(i => frequency.Id == i.Id);
            oldFrequency.Name = frequency.Name;
            oldFrequency.FrequencyCount = frequency.FrequencyCount;
            _db.SaveChanges();
        }
    }
}
