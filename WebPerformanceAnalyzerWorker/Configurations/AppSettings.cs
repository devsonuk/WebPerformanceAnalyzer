using WebPerformanceAnalyzerWorker.Configurations;

namespace WebPerformanceAnalyzerWorker.Configuration
{
    /// <summary>
    /// App Config
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public ConnectionString ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the API end point.
        /// </summary>
        /// <value>
        /// The API end point.
        /// </value>
        public ApiConfig ApiConfig { get; set; }

        /// <summary>
        /// Gets or sets the EmailOptions.
        /// </summary>
        /// <value>
        /// The EmailOptions.
        /// </value>
        public EmailOptions EmailOptions { get; set; }
    }
}
