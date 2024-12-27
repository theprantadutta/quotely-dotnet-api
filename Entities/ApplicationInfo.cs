using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quotely_dotnet_api.Entities;

public class ApplicationInfo
{
    [Key]
    [Column(Order = 0)]
    [MaxLength(100)]
    public string Id { get; set; } = null!;

    public bool MaintenanceBreak { get; set; }

    public string CurrentVersion { get; set; } = null!;

    public string AppUpdateUrl { get; set; } = null!;
}
