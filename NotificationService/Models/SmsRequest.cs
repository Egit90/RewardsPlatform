namespace NotificationService.Models;

public record SmsRequest(string PhoneNumber, string Message);