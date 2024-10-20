using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class Author
{
    [Key]
    [Column(Order = 0)] 
    [MaxLength(100)]
    public string Id { get; set; } = null!;

    [MaxLength(255)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Bio { get; set; } = null!;

    [MaxLength(200)]
    public string Description { get; set; } = null!;

    [MaxLength(200)]
    public string Link { get; set; } = null!;
    
    public int QuoteCount { get; set; }

    [MaxLength(200)] 
    public string Slug { get; set; } = null!;
    
    public DateTime DateAdded { get; set; }
    
    public DateTime DateModified { get; set; }
}