using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class Quote
{
    [Key]
    [Column(Order = 0)] 
    [MaxLength(200)]
    public string Id { get; set; } = null!;

    [MaxLength(200)]
    public string Author { get; set; } = null!;
    
    [MaxLength(400)]
    public string Content { get; set; } = null!;

    public string[] Tags { get; set; } = [];

    [MaxLength(100)]
    public string AuthorSlug { get; set; } = null!;

    public int Length { get; set; } 
    
    public DateTime DateAdded { get; set; }
    
    public DateTime DateModified { get; set; }
}