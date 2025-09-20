using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.Email;

public class FakeEmailSender : IEmailSender
{
    public Task<Result<string, Error>> Send(string to, string subject, string body)
    {
        Console.WriteLine($"[FAKE EMAIL] To: {to}, Subject: {subject}, Body: {body}");
        return Task.FromResult(Result.Success<string, Error>("fake-email-id-123"));
    }
}