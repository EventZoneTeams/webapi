using Services.BusinessModels.EmailModels;

namespace Services.Interface
{
    public interface IEmailService
    {
        Task SendEmail(Message message);

    }
}
