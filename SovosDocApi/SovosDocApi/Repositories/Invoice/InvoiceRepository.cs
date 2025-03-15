
using Microsoft.EntityFrameworkCore;
using System;

namespace SovosDocApi.Repositories.Invoice
{
    public class InvoiceRepository : Repository<InvoiceHeader>, IInvoiceRepository
    {
        public InvoiceRepository(SovosDbContext context) : base(context) { }


        public async Task<List<InvoiceHeader>> GetAllInvoicesAsync()
        {
            return await _context.InvoiceHeaders.ToListAsync();
        }


        public async Task<InvoiceHeader> GetInvoiceLinesAsync(string invoiceId)
        {
            return await _context.InvoiceHeaders.Where(s => s.InvoiceId == invoiceId).Include(s=>s.InvoiceLines).FirstOrDefaultAsync();
        }
    }
}
