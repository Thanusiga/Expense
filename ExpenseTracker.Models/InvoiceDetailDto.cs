using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class InvoiceDetailDto
    {
        // public int Id { get; set; }

        [Required, MinLength(3)]
        public String ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Double ProductPrice { get; set; }
    }
}