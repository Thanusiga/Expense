using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Pagination;
using ExpenseTracker.Server.Entities;

namespace ExpenseTracker.Server.Services
{
    public interface IExpenseTrackerService
    {
        Task<Invoice> AddInvoice(InvoiceDto invoice);

        Task<Invoice> UpdateInvoice(InvoiceUpdateDto invoice);

        Task<bool> DeleteInvoice(int id);

        Task<PaginatedData<List<Invoice>>> GetAllInvoices(PaginatedQuery paginatedQuery);

        Task<Invoice> GetInvoice(int id);
    }
}