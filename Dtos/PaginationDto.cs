namespace quotely_dotnet_api.Dtos;

public class PaginationDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItemCount { get; set; }
}
