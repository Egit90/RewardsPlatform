namespace NotificationService.Contracts;

public sealed record BroadcastRequest(
    string Subject,
    string Message,
    List<Channel> Channels,   
    List<string> Recipients  // phone numbers or email addresses
);