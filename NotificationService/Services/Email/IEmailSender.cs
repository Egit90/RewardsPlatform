using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.Email;

public interface IEmailSender
{
    Task<Result<string,Error>> Send(string email, string subject, string message);
}