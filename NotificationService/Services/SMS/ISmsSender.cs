using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.SMS;

public interface ISmsSender
{
    Task<Result<string,Error>> Send(string phoneNumber, string message);
}