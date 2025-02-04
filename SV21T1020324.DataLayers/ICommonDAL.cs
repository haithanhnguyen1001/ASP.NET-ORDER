﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.DataLayers
{
    public interface ICommonDAL<T> where T : class
    {
        IList<T> List(int page = 1, int pageSize = 0, string searchValues = "");

        int Count(string searchValue = "");

        T? Get(int id);

        int Add(T data);

        bool Update(T data);

        bool Delete(int id);

        bool InUsed(int id);
    }
}
