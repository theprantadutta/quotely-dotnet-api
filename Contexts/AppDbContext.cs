using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }

    public DbSet<Quote> Quotes { get; set; }

    public DbSet<QuoteOfTheDay> QuotesOfTheDays { get; set; }

    public DbSet<MotivationMonday> MotivationMondays { get; set; }

    public DbSet<DailyInspiration> DailyInspirations { get; set; }

    public DbSet<RandomQuote> RandomQuotes { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ApplicationInfo> ApplicationInfos { get; set; }

    // Read-only DbSet for the view
    public DbSet<QuoteOfTheDayWithQuote> QuoteOfTheDayWithQuotes { get; set; }
    public DbSet<MotivationMondayWithQuote> MotivationMondayWithQuotes { get; set; }
    public DbSet<DailyInspirationWithQuote> DailyInspirationWithQuotes { get; set; }

    public DbSet<AiFact> AiFacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tag Name is unique
        modelBuilder.Entity<Tag>().HasIndex(e => e.Name).IsUnique();

        // Map the view to the models
        modelBuilder
            .Entity<QuoteOfTheDayWithQuote>()
            .HasNoKey() // View do not have primary keys
            .ToView("View_QuoteOfTheDayWithQuote") //  the name of the view
            .HasKey(q => q.QuoteOfTheDayId);

        modelBuilder
            .Entity<DailyInspirationWithQuote>()
            .HasNoKey() // View do not have primary keys
            .ToView("View_DailyInspirationWithQuote") //  the name of the view
            .HasKey(q => q.DailyInspirationId);

        modelBuilder
            .Entity<MotivationMondayWithQuote>()
            .HasNoKey() // View do not have primary keys
            .ToView("View_MotivationMondayWithQuote") //  the name of the view
            .HasKey(q => q.MotivationMondayId);
    }
}
