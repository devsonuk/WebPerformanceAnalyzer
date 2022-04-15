using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebPerformanceAnalyzerWorker.Entities;
using WebPerformanceAnalyzerWorker.Enums;

namespace WebPerformanceAnalyzerWorker.Helpers
{
    public class FirebaseHelper<T> where T : BaseEntity
    {
        private readonly IFirebaseClient _firebaseClient;
        private readonly string _path;

        public FirebaseHelper()
        {

            if (typeof(T) == typeof(ScheduledTask))
            {
                _path = "ScheduledTasks/";
            }
            else if (typeof(T) == typeof(Report))
            {
                _path = "Reports/";
            }
            else if (typeof(T) == typeof(User))
            {
                _path = "Users/";
            }
            else
            {
                throw new ArgumentException();
            }

            var firebaseConfig = new FirebaseConfig()
            {
                AuthSecret = "Auth Secrect Key",
                BasePath = "Database BasePath"
            };

            _firebaseClient = new FirebaseClient(firebaseConfig);
        }

        public List<T> Get()
        {
            var response = _firebaseClient.GetAsync(_path).GetAwaiter().GetResult();
            Dictionary<string, T> data = default;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                data = response.ResultAs<Dictionary<string, T>>();
            }
            return data.Select(d =>
            {
                var value = d.Value;
                value.Id = d.Key;
                return value;
            }).ToList();
        }

        public T Set(string id, T data)
        {
            var response = _firebaseClient.SetAsync($"{_path}/{id}", data).GetAwaiter().GetResult();
            T result = default;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.ResultAs<T>();
            }
            return result;
        }

        public T Update(string id, T data)
        {
            var response = _firebaseClient.UpdateAsync($"{_path}/{id}", data).GetAwaiter().GetResult();
            T updatedData = default;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                updatedData = response.ResultAs<T>();
            }
            return updatedData;
        }

        public bool Delete(string id)
        {
            var response = _firebaseClient.DeleteAsync($"{_path}/{id}").GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public T Get(string id)
        {
            var response = _firebaseClient.GetAsync($"{_path}/{id}").GetAwaiter().GetResult();
            T data = default;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                data = response.ResultAs<T>();
            }
            data.Id = id;
            return data;
        }

        public string Add(T data)
        {
            var response = _firebaseClient.PushAsync(_path, data).GetAwaiter().GetResult();
            string id = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                id = response.ResultAs<dynamic>()?.name;
            }
            return id;
        }

        public string AddReport(Report report)
        {
            if (typeof(T) != typeof(Report))
            {
                throw new ArgumentException();
            }

            var request = new
            {
                report.TaskId,
                report.RequestedUrl,
                report.FinalUrl,
                report.Device,
                report.GatherMode,
                report.UserAgent,
                report.MetaData,
                //Audits = JsonConvert.SerializeObject(report.Audits),
                //Categories = JsonConvert.SerializeObject(report.Categories),
                //CategoryGroups = JsonConvert.SerializeObject(report.CategoryGroups),
                //Timings = JsonConvert.SerializeObject(report.Timings),
                report.Environment,
                report.Performance,
                report.Accessibility,
                report.BestPractices,
                report.SEO,
                report.PWA,
                report.BenchmarkIndex,
                report.FetchTime,
                report.HtmlReport
            };

            var response = _firebaseClient.PushAsync(_path, request).GetAwaiter().GetResult();
            string id = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                id = response.ResultAs<dynamic>()?.name;
            }
            return id;
        }

        public T GetLastReport(string taskId, LastReportType lastReportType)
        {
            if (typeof(T) != typeof(Report))
            {
                throw new ArgumentException(nameof(T));
            }

            var path = $"LastReports/{taskId}/{lastReportType}";
            var response = _firebaseClient.GetAsync(path).GetAwaiter().GetResult();
            var lastReportId = string.Empty;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                lastReportId = response.ResultAs<string>();
            }

            if (!string.IsNullOrEmpty(lastReportId))
            {
                return Get(lastReportId);
            }
            return null;
        }

        public bool SetLastReport(string taskId, LastReportType lastReportType, string reportId)
        {
            if (typeof(T) != typeof(Report))
            {
                throw new ArgumentException(nameof(T));
            }

            var path = $"LastReports/{taskId}/{lastReportType}";
            var response = _firebaseClient.SetAsync(path, reportId).GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
