namespace quotely_dotnet_api.Interfaces;

public interface IQuotableHttpService
{
    Task<TReturn?> GetAsync<TReturn>(string relativeUri);
}