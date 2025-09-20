using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.SMS;

public class FakeSmsSender: ISmsSender
{
    public Task<Result<string, Error>> Send(string phoneNumber, string message)
    {
        Console.WriteLine($"[FAKE SMS] To: {phoneNumber}, Message: {message}");
        return Task.FromResult(Result.Success<string, Error>("fake-message-id-123"));
    }
}