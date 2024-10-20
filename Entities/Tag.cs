using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class Tag
{
    [Key]
    [Column(Order = 0)] 
    [MaxLength(200)]
    public string Id { get; set; } = null!;

    [MaxLength(200)]
    public string Name { get; set; } = null!;
    
    [MaxLength(200)]
    public string Slug { get; set; } = null!;

    public int QuoteCount { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public DateTime DateModified { get; set; }
}