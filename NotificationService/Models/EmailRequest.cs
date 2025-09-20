namespace NotificationService.Models;

public sealed record EmailRequest(string Email, string Subject, string Message);