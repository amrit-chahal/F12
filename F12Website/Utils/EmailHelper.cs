using F12Website.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;

using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace F12Website.Utils
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;
        private readonly ValidateReCaptcha _validateReCaptcha;

        public EmailHelper(IConfiguration config, ValidateReCaptcha validateReCaptcha)
        {
            _config = config;
            _validateReCaptcha = validateReCaptcha;

        }



        public async Task<string> SendEmailAsync(ContactFormModel contactFormModel)
        {
            var secretKey = _config.GetValue<string>("ReCaptchaSecretkey");
            var IsUserNotABot = await _validateReCaptcha.IsCaptchaValid(secretKey, contactFormModel.Token);
            if (IsUserNotABot == true)
            {
                var serviceAccountEmail = _config.GetValue<string>("GServiceEmail");
                var privatekeys = Convert.FromBase64String(_config["GoogleEmailSenderCertificate"]);
                var certificate = new X509Certificate2(privatekeys, (string)null, X509KeyStorageFlags.MachineKeySet);
                var credentials = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail) { User = _config.GetValue<string>("ServiceAccountUser"), Scopes = new[] { GmailService.Scope.GmailSend } }.FromCertificate(certificate));

                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "EmailSender"

                });



                var mailMessage = new MailMessage
                {
                    Subject = "PropMate Query from : " + contactFormModel.Name,
                    Body = $"<h4>{contactFormModel.Subject}</h4><br>" +
                    $"<p><b>Email: </b> {contactFormModel.Email}</p><br>" +
                    $"<p><b>Message: </b>{ contactFormModel.Message}</p>"



                };
                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(_config.GetValue<string>("AZURE_USERNAME"));
                MimeMessage mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);
                byte[] rawMimeMessage;
                using (var stream = new MemoryStream())
                {
                    mimeMessage.WriteTo(stream);
                    rawMimeMessage = stream.ToArray();

                }



                var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = Base64UrlEncode(rawMimeMessage)
                };


                var request = service.Users.Messages.Send(gmailMessage, "me");

                await request.ExecuteAsync();



                return "Message Sent";

            }
            return "Something went wrong. Please try again";




        }
        public static string Base64UrlEncode(byte[] bytes)
        {

            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

    }
}
