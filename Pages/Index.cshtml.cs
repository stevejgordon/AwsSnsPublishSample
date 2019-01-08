using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SnsPublishSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAmazonSimpleNotificationService _simpleNotificationService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IAmazonSimpleNotificationService simpleNotificationService, ILogger<IndexModel> logger)
        {
            _simpleNotificationService = simpleNotificationService;
            _logger = logger;
        }

        public async Task OnGet()
        {
            var request = new PublishRequest
            {
                Message = $"Test at {DateTime.UtcNow.ToLongDateString()}",
                TopicArn = "arn:aws:sns:eu-west-2:01234567890:SampleTopic",
            };

            try
            {
                var response = await _simpleNotificationService.PublishAsync(request);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Successfully sent SNS message '{response.MessageId}'");
                }
                else
                {
                    _logger.LogWarning(
                        $"Received a failure response '{response.HttpStatusCode}' when sending SNS message '{response.MessageId ?? "Missing ID"}'");
                }
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                _logger.LogError(ex, "An AWS SNS exception was thrown");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown");
            }
        }
    }
}
