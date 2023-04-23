using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Pagination;
using ExpenseTracker.Server.Entities;
using ExpenseTracker.Server.Services;
using Microsoft.Extensions.Primitives;

namespace ExpenseTracker.Server.Helpers
{
    public class TemplateGenerator
    {
        public static string GetHTMLString(PaginatedData<List<Invoice>> invoice, PaginatedQuery query)
        {
            var yy = invoice.Data.SelectMany(x => x.InvoiceDetails).Sum(e => e.Quantity * e.ProductPrice);
            int i = 1;

            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body>
                              <H2 class='header'> Expense Details </H2>
                            <div><strong> Total Price : {0} </strong>
                            <div><strong> Total Count of invoice : {1} </strong>
                            <div><strong> Start date : {2} </strong>
                            <div><strong> End date : {3} </strong>
                                ", yy, invoice.MetaData.TotalCount, query.Start, query.End);
            foreach (var inv in invoice.Data)
            {
                sb.AppendFormat(@"
                                <H4> Invoice - {0} </H4>
                                        <div class='box align-center'>
                                            <div> Invoice num  - {1} </div>
                                            <div> Date  - {2} </div>
                                            <br>
                                            <H5> product details </H5>
                                            <table>
                                                <thead>
                                                    <th> Product name</th>
                                                    <th> Product price</th>
                                                    <th> Quantity</th>
                                                    <th> Total</th>
                                                </thead> 
                                                <tbody>
                               ", i, inv.InvoiceNo, inv.DateTime);
                foreach (var det in inv.InvoiceDetails)
                {
                    sb.AppendFormat(@"<tr>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                    </tr>", det.ProductName, det.ProductPrice, det.Quantity, det.Quantity * det.ProductPrice);

                }
                sb.AppendFormat(@"
                                </tbody>
                            </table>
                            Total : {0}
                        </div> ", inv.InvoiceDetails.Sum(x => x.ProductPrice * x.Quantity));

                i++;
            }
            sb.AppendFormat(@" 
                            </body>
                        </html>");
            return sb.ToString();
        }
        public static string GetHTMLStringForInvoice(Invoice invoice)
        {

            var sb = new StringBuilder();
            sb.AppendFormat(@"
             <html>
                            <head>
                            </head>
                            <body>
                              <H2 class='header'> Expense Details </H2>
                                <div class='box align-center'>
                                <div> Invoice num  - {0} </div>
                                <div> Date  - {1} </div>
                                <br>
                                <H5> product details </H5>
                                <table>
                                    <thead>
                                        <th> Product name</th>
                                        <th> Product price</th>
                                        <th> Quantity</th>
                                        <th> Total</th>
                                    </thead> 
                                    <tbody>
                               ", invoice.InvoiceNo, invoice.DateTime);

            foreach (var det in invoice.InvoiceDetails)
            {
                sb.AppendFormat(@"<tr>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                    </tr>", det.ProductName, det.ProductPrice, det.Quantity, det.Quantity * det.ProductPrice);

            }
            sb.AppendFormat(@"
                                </tbody>
                            </table>
                            Total : {0}
                        </div> ", invoice.InvoiceDetails.Sum(x => x.ProductPrice * x.Quantity));

            sb.AppendFormat(@" 
                             </div> 
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}