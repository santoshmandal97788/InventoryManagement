using UI.ViewModel;

namespace UI.Services.IService
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, EmployeeViewModel emp);
    }
}
