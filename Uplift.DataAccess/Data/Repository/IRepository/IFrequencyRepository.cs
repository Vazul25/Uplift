using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository : IRepository<Frequency>
    {
        public void Update(Frequency frequency);
        public IEnumerable<SelectListItem> GetFrequencyListForDropDown();
    }
}
