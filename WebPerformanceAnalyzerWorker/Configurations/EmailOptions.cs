namespace WebPerformanceAnalyzerWorker.Configuration
{
    /// <summary>
    /// Represents EmailOptions class
    /// </summary>
    public class EmailOptions
    {


        /// <summary>
        /// Gets or sets the ClientFromAddress
        /// </summary>
        /// <value>
        /// The ClientFromAddress
        /// </value>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the SmtpEnableSsl
        /// </summary>
        /// <value>
        /// The SmtpEnableSsl
        /// </value>
        public bool IsSslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the ClientSmtpHost
        /// </summary>
        /// <value>
        /// The ClientSmtpHost
        /// </value>
        public string SmtpHost { get; set; }

        /// <summary>
        /// Gets or sets the ClientSmtpUserName
        /// </summary>
        /// <value>
        /// The ClientSmtpUserName
        /// </value>
        public string SmptpUserName { get; set; }

        /// <summary>
        /// Gets or sets the ClientSmtpPassword
        /// </summary>
        /// <value>
        /// The ClientSmtpPassword
        /// </value>
        public string SmptpPassword { get; set; }
    }
}
