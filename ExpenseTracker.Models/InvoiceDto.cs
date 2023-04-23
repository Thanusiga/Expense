using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class InvoiceDto
    {
        [Required, MinLength(3)]
        public String InvoiceNo { get; set; }
        [Required]
        public DateTime? DateTime { get; set; }

        [Required, MinLength(1)]
        public List<InvoiceDetailDto> InvoiceDetails { get; set; } = new List<InvoiceDetailDto>() { };
    }
}