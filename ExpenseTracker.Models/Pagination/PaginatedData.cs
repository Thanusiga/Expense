using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models.Pagination
{

    public class PaginatedData<T> where T : class
    {
        public T Data { get; set; }

        public MetaData MetaData { get; set; }
    }

    public class MetaData
    {

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

    }
}