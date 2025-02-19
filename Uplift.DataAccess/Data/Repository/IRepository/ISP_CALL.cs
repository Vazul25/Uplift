﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface ISP_CALL: IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null);
        void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null);
        T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null);
    }
}
