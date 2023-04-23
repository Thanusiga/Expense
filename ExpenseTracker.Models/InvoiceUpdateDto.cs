using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class InvoiceUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public String InvoiceNo { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        public List<InvoiceDetailDto> InvoiceDetails { get; set; }
    }
}