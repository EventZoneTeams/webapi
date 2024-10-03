using static EventZone.Services.Services.PayOSObjects;

namespace EventZone.Services.Interface
{
    public interface IPayOSService
    {
        Task<string> CreateLink(int depositMoney);
        Task<PayOSWebhookResponse> ReturnWebhook(PayOSWebhook payOSWebhook);
    }
}
