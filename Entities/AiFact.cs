using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class AiFact
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }

    [MaxLength(400)]
    [Column(TypeName = "text")]
    public string Content { get; set; } = null!;

    [MaxLength(200)]
    public string AiFactCategory { get; set; } = null!;

    [MaxLength(100)]
    [DefaultValue("Unknown")]
    public string Provider { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime DateModified { get; set; }
}
