using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quotely_dotnet_api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteOfTheDayView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW ""View_QuoteOfTheDayWithQuote"" AS
                SELECT 
                    qotd.""Id"" AS ""QuoteOfTheDayId"",
                    qotd.""QuoteDate"",
                    qotd.""DateAdded"" AS ""QuoteOfTheDayDateAdded"",
                    qotd.""DateModified"" AS ""QuoteOfTheDayDateModified"",
                    q.""Id"" AS ""QuoteId"",
                    q.""Author"",
                    q.""Content"",
                    q.""Tags"",
                    q.""AuthorSlug"",
                    q.""Length"",
                    q.""DateAdded"" AS ""QuoteDateAdded"",
                    q.""DateModified"" AS ""QuoteDateModified""
                FROM 
                    ""QuotesOfTheDays"" qotd
                INNER JOIN 
                    ""Quotes"" q ON qotd.""QuoteId"" = q.""Id"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ""View_QuoteOfTheDayWithQuote"";");
        }
    }
}
