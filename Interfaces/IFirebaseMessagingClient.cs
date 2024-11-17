namespace quotely_dotnet_api.Interfaces;

/// <summary>
/// FOR FIREBASE MESSAGING CLIENT
/// </summary>
public interface IFirebaseMessagingClient
{
    /// THIS IS USED TO SEND THE NOTIFICATION
    Task<string?> SendNotification(
        string topic,
        string title,
        string body,
        IReadOnlyDictionary<string, string>? data
    );
}
