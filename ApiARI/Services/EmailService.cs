using ApiARI.Model;
using ApiARI.Utilities;
using MimeKit;
using ApiARI.Interfaces;

namespace ApiARI.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailFactory _mailFactory;

        public EmailService(MailFactory mailFactory)
        {
            _mailFactory = mailFactory;
        }

        public Task<bool> SendReceptionCertificate(EmailBody emailBody)
        {
            try
            {
                MimeMessage mimeMessage = new();
                MimeMessage mimeMessageAutoReplay = new();
                mimeMessage.To.Add(new MailboxAddress(emailBody.Name, "contacto@ari-pro.com"));
                mimeMessageAutoReplay.To.Add(new MailboxAddress(emailBody.Name, emailBody.Email));

                mimeMessage.Subject = "Nuevo Cliente";
                mimeMessageAutoReplay.Subject = "Solicitud Recibida";

                var bodyBuilder = new BodyBuilder();
                var bodyBuilderAutoReplay = new BodyBuilder();

                bodyBuilder.TextBody = $"Nombre: {emailBody.Name} \nApellidos: {emailBody.LastName} \nCorreo: {emailBody.Email} \nTeléfono: {emailBody.Phone} \nEmpresa: {emailBody.Company} \nMensaje: {emailBody.Text} \nPaquete: {emailBody.Package} \nDemostración: {emailBody.Demo} \nPlan: {emailBody.Plan}";
                bodyBuilderAutoReplay.HtmlBody = $"<div style=\"width: 100%; height: 6rem; background-color: #2d297e;\"><img style=\"width: 17rem; height: 100%;\" src=\"https://arigatewaytest.azurewebsites.net/assets/img/PrimerLogoARIblanco.png\"></div>\r\n<p><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">Hola <span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">{emailBody.Name} 🏠.</span></span></span></span></p>\r\n<p><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">Hemos recibido tu solitud de contacto.</span></span></span></span></p>\r\n<p><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">Muchas gracias por tu inter&eacute;s en el aplicativo ARI (Acta de Recepci&oacute;n de Inmueble).</span></span></span></span></p>\r\n<p><span style=\"font-size: 14pt;\"><strong><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">&iquest;Cu&aacute;l es el siguiente paso?</span></span></span></span></strong></span></p>\r\n<p><span style=\"font-size: 12pt;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">A la brevedad uno de nuestros agentes se pondr&aacute; en contacto contigo para agendar una demostraci&oacute;n y encontrar la soluci&oacute;n que se ajuste a las necesidades de su equipo.</span></span></span></span></span></p>\r\n<p><span style=\"font-size: 12pt;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">__________________________________</span></span></span></span></span></p>\r\n<p><span style=\"font-size: 8pt;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\"><span style=\"vertical-align: inherit;\">Copyright <strong>&copy;&nbsp;</strong>2023 ARI, ACTAS DE RECEPCI&Oacute;N DE INMUEBLE.</span></span></span></span></span></p>";

                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessageAutoReplay.Body = bodyBuilderAutoReplay.ToMessageBody();

                _mailFactory.MailSender(mimeMessage);
                _mailFactory.MailSender(mimeMessageAutoReplay);

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }
    }
}
