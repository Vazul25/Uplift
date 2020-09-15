using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Uplift.DataAccess.Data.Initializer
{
    public interface IDBInitializer
    {
        void Initialize();
        Task InitializeAsync();
    }
}
