using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class RandomQuote
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }

    public DateTime QuoteDate { get; set; }

    [MaxLength(200)]
    public string QuoteId { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime DateModified { get; set; }
}
