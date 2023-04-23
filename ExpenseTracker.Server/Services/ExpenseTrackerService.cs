using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Pagination;
using ExpenseTracker.Server.AppDbContext;
using ExpenseTracker.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Server.Services
{
    public class ExpenseTrackerService : IExpenseTrackerService
    {
        public readonly ExpenseTrackerDbContext _expenseTrackerDbContext;
        public ExpenseTrackerService(ExpenseTrackerDbContext expenseTrackerDbContext)
        {
            _expenseTrackerDbContext = expenseTrackerDbContext;
        }

        public async Task<Invoice> AddInvoice(InvoiceDto invoice)
        {
            if (await _expenseTrackerDbContext.Invoices.AsNoTracking().FirstOrDefaultAsync(x => x.InvoiceNo == invoice.InvoiceNo) != null)
            {
                throw new System.Exception($"{invoice.InvoiceNo} is already exist");
            }

            var inv = new Invoice()
            {
                InvoiceNo = invoice.InvoiceNo,
                DateTime = invoice.DateTime.HasValue
                          ? invoice.DateTime.Value
                          : DateTime.Now,
                InvoiceDetails = invoice.InvoiceDetails.Select(x => new ExpenseTracker.Server.Entities.InvoiceDetail()
                {
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    ProductPrice = x.ProductPrice,
                }).ToList(),
            };

            var result = await _expenseTrackerDbContext.Invoices.AddAsync(inv);
            await _expenseTrackerDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            var cls = await _expenseTrackerDbContext.Invoices.FindAsync(id);

            if (cls == null)
            {
                throw new System.Exception($"{id} is does not exist");
            }

            var res = _expenseTrackerDbContext.Invoices.Remove(cls);

            await _expenseTrackerDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedData<List<Invoice>>> GetAllInvoices(PaginatedQuery paginatedQuery)

        {
            DateTime startDate;
            DateTime endDate;

            var query = _expenseTrackerDbContext.Invoices.Include(x => x.InvoiceDetails).AsQueryable().AsNoTracking();

            if (paginatedQuery.SortDirection == "asc")
            {
                query = query.OrderBy(d => d.Id);
            }


            if (paginatedQuery.Start != null && paginatedQuery.End != null)
            {
                query = query.Where(x => paginatedQuery.Start <= x.DateTime && paginatedQuery.End >= x.DateTime);
            }
            else if (paginatedQuery.Start != null)
            {
                query = query.Where(x => paginatedQuery.Start <= x.DateTime);
            }
            else if (paginatedQuery.End != null)
            {
                query = query.Where(x => paginatedQuery.End >= x.DateTime);
            }

            if (paginatedQuery.SortDirection == "asc")
            {
                query = query.OrderBy(d => d.Id);
            }

            var invoices = await PaginatedList<Invoice>.CreateAsync(query, paginatedQuery.Page, paginatedQuery.PerPage);

            return new PaginatedData<List<Invoice>>()
            {
                Data = invoices,
                MetaData = new MetaData()
                {
                    TotalPages = invoices.TotalPages,
                    CurrentPage = invoices.PageIndex,
                    TotalCount = invoices.TotalCount,
                }
            };

        }

        public async Task<Invoice> GetInvoice(int id)
        {
            return await _expenseTrackerDbContext.Invoices.Include(x => x.InvoiceDetails).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Invoice> UpdateInvoice(InvoiceUpdateDto invoice)
        {

            var inv = await _expenseTrackerDbContext.Invoices
            .Include(x => x.InvoiceDetails)
            .FirstOrDefaultAsync(x => x.Id == invoice.Id);

            if (inv == null)
            {
                throw new System.NullReferenceException($"{invoice.Id} is not found");
            }
            if (inv.InvoiceNo != invoice.InvoiceNo)
            {
                throw new System.Exception($"You can not update invoice number");
            }

            inv.DateTime = invoice.DateTime;
            inv.InvoiceDetails = invoice.InvoiceDetails.Select(x => new ExpenseTracker.Server.Entities.InvoiceDetail()
            {
                InvoiceId = invoice.Id,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                ProductPrice = x.ProductPrice,
            }).ToList();
            _expenseTrackerDbContext.Invoices.Update(inv);
            await _expenseTrackerDbContext.SaveChangesAsync();

            return inv;
        }
    }
}