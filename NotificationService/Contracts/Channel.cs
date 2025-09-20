using System.Text.Json.Serialization;

namespace NotificationService.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Channel
{
    Sms,
    Email
}