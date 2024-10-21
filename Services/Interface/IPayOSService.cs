using EventZone.Services.Services;
using Net.payOS.Types;

namespace EventZone.Services.Interface
{
    public interface IPayOSService
    {
        Task<string> CreateLink(int depositMoney, Guid txnRef);
        Task<WebhookResponse> ReturnWebhook(WebhookType webhookType);
    }
}
