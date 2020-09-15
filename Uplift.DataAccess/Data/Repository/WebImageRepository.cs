using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class WebImageRepository : Repository<WebImage>, IWebImageRepository
    {

        public WebImageRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
