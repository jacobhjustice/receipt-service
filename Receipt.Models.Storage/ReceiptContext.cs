using Microsoft.EntityFrameworkCore;

namespace Receipt.Models.Storage;

public class ReceiptContext : DbContext
{
    public virtual DbSet<Data.Receipt> Receipts { get; set; }
    public virtual DbSet<Data.ReceiptItem> ReceiptItems { get; set; }
    
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Data.ReceiptItem>()
            .HasOne<Data.Receipt>()
            .WithMany()
            .HasForeignKey(x => x.ReceiptId);
    }
}