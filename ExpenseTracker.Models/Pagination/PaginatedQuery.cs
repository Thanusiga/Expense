using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models.Pagination
{
    public class PaginatedQuery
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        // public string Keyword { get; set; }

        [RegularExpression("asc|desc", ErrorMessage = "SortDirection should be asc or desc")]
        public string SortDirection { get; set; }

        public string SortBy { get; set; } = "Id";

        // public int Status { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}