using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.Email;

public class SesEmailSender(IAmazonSimpleEmailServiceV2 sesClient) : IEmailSender
{
    public async Task<Result<string, Error>> Send(string email, string subject, string message)
    {
        try
        {
            var request = SendEmailRequest(email, subject, message);

            var response = await sesClient.SendEmailAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK 
                ? Result.Success<string, Error>(response.MessageId) 
                : Result.Failure<string, Error>(Error.Failure("ses.failed", $"SES returned {response.HttpStatusCode}"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string, Error>(Error.Failure("ses.exception", ex.Message));
        }
    }

    private static SendEmailRequest SendEmailRequest(string email, string subject, string message)
    {
        var request = new SendEmailRequest
        {
            Destination = new Destination
            {
                ToAddresses = [email]
            },
            Content = new EmailContent
            {
                Simple = new Message()
                {
                    Subject = new Content() { Data = subject },
                    Body = new Body() { Text = new Content() { Data = message } }
                }
            },
            FromEmailAddress = "dsa@myemailaddress.com"
        };
        return request;
    }
}