using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using WebPerformanceAnalyzerWorker.Entities;

namespace WebPerformanceAnalyzerWorker.Helpers
{
    public class DbHelper
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        private readonly int _agencyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbHelper" /> class.
        /// </summary>
        /// <param name="connectionstring">parsing a connection string</param>
        public DbHelper(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public void SaveData(Report data)
        {
			var audits = JsonConvert.SerializeObject(data.Audits);
			var categories = JsonConvert.SerializeObject(data.Categories);
			var categoryGroups = JsonConvert.SerializeObject(data.CategoryGroups);
			var timings = JsonConvert.SerializeObject(data.Timings);
			var environment = JsonConvert.SerializeObject(data.Environment);
			var metaData = JsonConvert.SerializeObject(data.MetaData);

			var sql = $@"
                    INSERT INTO Foundation.PerformanceMetrics (
					AppDetailId
					,RequestedUrl
					,FinalUrl
					,GatherMode
					,UserAgent
					,[Date]
					,Audits
					,Categories
					,CategoryGroups
					,Timings
					,Environment
					,MetaData
					,Performance
					,Accessibility
					,BestPractices
					,SEO
					,PWA
					,BenchmarkIndex
					,HtmlReport
					)
				VALUES (
					@AppDetailId
					,@RequestedUrl
					,@FinalUrl
					,@GatherMode
					,@UserAgent
					,@FetchTime
					,@Audits
					,@Categories
					,@CategoryGroups
					,@Timings
					,@Environment
					,@MetaData
					,@Performance
					,@Accessibility
					,@BestPractices
					,@SEO
					,@PWA
					,@BenchmarkIndex
					,@HtmlReport
					)
					";
			using (var con = new SqlConnection(_connectionString))
			{
				con.Execute(
					sql,
					new {data.TaskId, 
						data.RequestedUrl, 
						data.FinalUrl, 
						data.GatherMode, 
						data.UserAgent, 
						data.FetchTime, 
						Audits = JsonConvert.SerializeObject(data.Audits), 
						Categories = JsonConvert.SerializeObject(data.Categories), 
						CategoryGroups = JsonConvert.SerializeObject(data.CategoryGroups),
						Timings = JsonConvert.SerializeObject(data.Timings),
						Environment = JsonConvert.SerializeObject(data.Environment),
						data.MetaData,
						data.Performance,
						data.Accessibility,
						data.BestPractices,
						data.SEO,
						data.PWA,
						data.BenchmarkIndex,
						data.HtmlReport
					},
					commandType: CommandType.Text,
					commandTimeout: 6000);
			}

		}
    }
}
