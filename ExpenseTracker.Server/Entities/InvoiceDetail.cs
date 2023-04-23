using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Server.Entities
{
    public class InvoiceDetail : AuditableEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        [Required]

        public string ProductName { get; set; }
        [Required]

        public int Quantity { get; set; }
        [Required]

        public double ProductPrice { get; set; }

    }
}