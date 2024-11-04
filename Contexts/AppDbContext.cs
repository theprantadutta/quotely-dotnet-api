using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<Quote> Quotes { get; set; }
    
    public DbSet<QuoteOfTheDay> QuotesOfTheDays { get; set; }
    
    public DbSet<Tag> Tags { get; set; }
    
    // Read-only DbSet for the view
    public DbSet<QuoteOfTheDayWithQuote> QuoteOfTheDayWithQuotes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tag Name is unique
        modelBuilder.Entity<Tag>()
            .HasIndex(e => e.Name)
            .IsUnique();
        
        // Map the view to the model
        modelBuilder.Entity<QuoteOfTheDayWithQuote>()
            .HasNoKey() // Views typically do not have primary keys
            .ToView("View_QuoteOfTheDayWithQuote") // Specify the name of the view
            .HasKey(q => q.QuoteOfTheDayId);
    }
}