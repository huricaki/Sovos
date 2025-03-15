using Microsoft.EntityFrameworkCore;

public class SovosDbContext : DbContext
{
    public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }

    public SovosDbContext(DbContextOptions<SovosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // burada InvoiceHeader ile InvoiceLine 1-n ilişkisini belirtiyoruz
        modelBuilder.Entity<InvoiceLine>()
            .HasOne(il => il.InvoiceHeader)
            .WithMany(ih => ih.InvoiceLines)
            .HasForeignKey(il => il.InvoiceId);

        base.OnModelCreating(modelBuilder);
    }
}
