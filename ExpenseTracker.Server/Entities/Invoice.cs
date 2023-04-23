using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Server.Entities
{
    public class Invoice : AuditableEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public String InvoiceNo { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        public List<InvoiceDetail> InvoiceDetails { get; set; }

    }
}