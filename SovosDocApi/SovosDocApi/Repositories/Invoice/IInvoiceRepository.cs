namespace SovosDocApi.Repositories.Invoice
{
    public interface IInvoiceRepository : IRepository<InvoiceHeader>
    {
        Task<List<InvoiceHeader>> GetAllInvoicesAsync();
        Task<InvoiceHeader> GetInvoiceLinesAsync(string invoiceId);

    }
}
