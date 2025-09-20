using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CSharpFunctionalExtensions;
using SharedKernel.Errors;

namespace NotificationService.Services.SMS;

public class SmsSender(IAmazonSimpleNotificationService snsClient) : ISmsSender
{
    public async Task<Result<string, Error>> Send(string phoneNumber, string message)
    {
        try
        {
            var request = new PublishRequest
            {
                Message = message,
                PhoneNumber = phoneNumber
            };
            
            var response = await snsClient.PublishAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result.Failure<string, Error>(
                    MainErrors.NotificationFailure(
                        $"SNS returned HTTP {response.HttpStatusCode}, RequestId: {response.ResponseMetadata?.RequestId}"
                    )
                );
            }

            return Result.Success<string, Error>(response.MessageId);
        }
        catch (Exception ex)
        {
            return Result.Failure<string, Error>(
                MainErrors.NotificationFailure($"Exception: {ex.Message}")
            );
        }
    }
}