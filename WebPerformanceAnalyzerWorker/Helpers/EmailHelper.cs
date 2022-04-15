using System.Net.Mail;
using WebPerformanceAnalyzerWorker.Configuration;


namespace ApprovedBuyInvoiceWorker.Helper
{
    

    /// <summary>
    /// Represents EmailHelper class
    /// </summary>
    public class EmailHelper
    {
        private readonly EmailOptions _emailOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHelper"/> class.
        /// </summary>
        /// <param name="businessProfileId">businessProfileId</param>
        /// <param name="emailOptions">emailOptions</param
        public EmailHelper(EmailOptions emailOptions)
        {
            _emailOptions = emailOptions;
        }

        /// <summary>
        /// send email to client
        /// </summary>
        /// <param name="mailMessage">mailMessage</param>
        public void SendHTMLMail(MailMessage mailMessage)
        {
            using (var smtpClient = new SmtpClient(_emailOptions.SmtpHost, 587))
            {
                smtpClient.EnableSsl = _emailOptions.IsSslEnabled;
                smtpClient.Timeout = int.MaxValue;
                smtpClient.Credentials = new System.Net.NetworkCredential(
                    _emailOptions.SmptpUserName,
                    _emailOptions.SmptpPassword);
                smtpClient.Send(mailMessage);
            }
        }
    }
}
