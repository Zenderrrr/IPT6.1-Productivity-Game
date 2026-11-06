namespace FocusUp.Application.Interfaces
{
    public interface IEmailService
    {
        System.Threading.Tasks.Task SendEmailAsync(string recipient, string subject, string text);
    }
}