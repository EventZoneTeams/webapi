using EventZone.Services.Services;
using Net.payOS.Types;

namespace EventZone.Services.Interface
{
    public interface IPayOSService
    {
        Task<string> CreateLink(int depositMoney, int orderCode);
        Task<WebhookResponse> ReturnWebhook(WebhookType webhookType);
    }
}
