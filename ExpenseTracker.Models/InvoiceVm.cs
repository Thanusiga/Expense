using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class InvoiceVm
    {
        public int Id { get; set; }
        public bool ShowProduct { get; set; } = false;

        public String InvoiceNo { get; set; }

        public DateTime? DateTime { get; set; }

        public List<InvoiceDetailDto> InvoiceDetails { get; set; }
    }
}