using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

/// <summary>
/// THIS CLASS WILL BE USED TO SEND NOTIFICATION TO ANDROID/IOS
/// </summary>
public class FirebaseMessagingClient : IFirebaseMessagingClient
{
    private readonly FirebaseMessaging _messaging;

    /// <summary>
    /// THE CONSTRUCTOR, WE ARE CONSTRUCTING THE FIREBASE APP HERE
    /// </summary>
    public FirebaseMessagingClient()
    {
        var googleFile =
            AppContext.BaseDirectory + "/Sensitive/quotely-app.json";

        if (File.Exists(googleFile) == false)
        {
            throw new NotImplementedException("Please Provide A Google File Extension");
        }

        var app = FirebaseApp.DefaultInstance;
        if (FirebaseApp.DefaultInstance == null)
        {
            app = FirebaseApp.Create(
                new AppOptions()
                {
                    Credential = GoogleCredential
                        .FromFile(googleFile)
                        .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                }
            );
        }
        _messaging = FirebaseMessaging.GetMessaging(app);
    }

    private static Message CreateNotification(
        string topic,
        string title,
        string notificationBody,
        IReadOnlyDictionary<string, string>? data
    )
    {
        return new Message()
        {
            // Token = token,
            Topic = topic,
            Notification = new Notification { Body = notificationBody, Title = title },
            Data = data
        };
    }

    /// <summary>
    /// THIS SENDS THE NOTIFICATION
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task<string?> SendNotification(
        string topic,
        string title,
        string body,
        IReadOnlyDictionary<string, string>? data
    )
    {
        return await _messaging.SendAsync(CreateNotification(topic, title, body, data));
    }
}

