using Ecom.Core.DTO;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Using SMTP 
        public async Task SendEmail(EmailDTO emailDTO)
        {
            // Implement SMTP email
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Ecomm-App" , _configuration["EmailSetting:From"]));
            message.Subject = emailDTO.Subject; 
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    //connect to smtp server
                                // host, port, useSsl
                    await client.ConnectAsync( _configuration["EmailSetting:Smtp"], 
                        int.Parse( _configuration["EmailSetting:Port"]), true);
                    await client.AuthenticateAsync( _configuration["EmailSetting:UserName"], 
                        _configuration["EmailSetting:Password"]);
                    await client.SendAsync(message);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

            //configuration.GetSection("EmailSettings");


        }
    }
}
