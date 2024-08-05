using Domain.DTOs.EmailModels;

namespace Services.Interface
{
    public interface IEmailService
    {
        Task SendEmail(Message message);

        Task SendHTMLEmail(Message message);
    }
}