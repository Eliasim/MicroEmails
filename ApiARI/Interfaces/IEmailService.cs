using ApiARI.Model;

namespace ApiARI.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendReceptionCertificate(EmailBody emailBody);
    }
}
