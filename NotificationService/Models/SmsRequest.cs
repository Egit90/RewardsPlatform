namespace NotificationService.Models;

public sealed record SmsRequest(string PhoneNumber, string Message);