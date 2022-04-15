using System;
using DeltaX.Services.Core;
using DeltaX.Services.Core.Interfaces;
using WebPerformanceAnalyzerWorker.Configuration;
using Microsoft.Extensions.Options;
using WebPerformanceAnalyzerWorker.Helpers;
using System.Collections.Generic;
using WebPerformanceAnalyzerWorker.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using log4net;
using System.Reflection;
using ApprovedBuyInvoiceWorker.Helper;
using WebPerformanceAnalyzerWorker.Models;
using System.IO;
using System.Net.Mail;
using RazorLight;
using System.Text;
using WebPerformanceAnalyzerWorker.Enums;
using System.Globalization;
using System.Threading;

namespace WebPerformanceAnalyzerWorker
{
    /// <summary>
    /// Worker Class
    /// </summary>
    public class Worker : BaseWorker
    {

        private readonly DbHelper _dbHelper;
        private readonly EmailHelper _emailHelper;
        private readonly AppSettings _appSettings;
        private readonly LightHouseHelper _lightHouseHelper;
        private readonly FirebaseHelper<User> _userRepository;
        private readonly FirebaseHelper<Report> _reportRepository;
        private readonly FirebaseHelper<ScheduledTask> _taskRepository;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Url2 { get; set; }
        public int UserId { get; set; }
        public Dictionary<string, long> _performanceStats { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Worker" /> class.
        /// </summary>
        /// <param name="connectionStringFactory">The connection string factory.</param>
        /// <param name="pendingTaskRepository">The pending task repository.</param>
        public Worker(IConnectionStringFactory connectionStringFactory, IPendingTaskRepository pendingTaskRepository)
            : base(connectionStringFactory, pendingTaskRepository)
        {
            _appSettings = ServiceProviderHelper.GetService<IOptions<AppSettings>>().Value;
            _lightHouseHelper = new LightHouseHelper();
            _dbHelper = new DbHelper(_appSettings.ConnectionString.DeltaXCore);
            _emailHelper = new EmailHelper(_appSettings.EmailOptions);
            _taskRepository = new FirebaseHelper<ScheduledTask>();
            _reportRepository = new FirebaseHelper<Report>();
            _userRepository = new FirebaseHelper<User>();
            _performanceStats = new Dictionary<string, long>();
        }

        /// <summary>
        /// Does the task.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// boolean
        /// </returns>
        public override bool DoTask(string arguments)
        {
            try
            {
                Log.Warn("Worker Started ...");
                ValidateArguments(arguments);
                ProcessAllTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return true;
        }

        private void TestMethod()
        {
            var reportIds = new List<string>() { "-MwJdosmmUnWLqnxpltN", "-MwPI5qSgB3YMrrwW7iE", "-MwPO94a4v0HwUemhC94" };
            var taskId = "-MwJZMf623H-rbzi5ghZ";
            //var reports = _reportRepository.Get(taskId, 10);
            //var lastReports = (
            //       from report in _reportRepository.Get()
            //       where report.FetchTime >= DateTime.UtcNow.AddDays(-30) && report.FetchTime < DateTime.UtcNow
            //       orderby report.FetchTime
            //       select report
            //   ).ToList();
            var lastReports = _reportRepository.Get()
                                                .Where(r => r.TaskId == taskId && r.FetchTime.Date >= DateTime.UtcNow.AddDays(-30).Date && r.FetchTime.Date < DateTime.UtcNow.Date)
                                                .OrderBy(r => r.FetchTime)
                                                .ToList();
            var req2 = lastReports.Where(r => r.FetchTime.Date >= DateTime.UtcNow.AddDays(-30).Date);
            Console.WriteLine(lastReports.Count);
        }

        private void SaveReportData(Report report)
        {
            _ = _reportRepository.AddReport(report);
        }

        private void ValidateArguments(string arguments)
        {
            var jsonArg = JObject.Parse(arguments);
            if (int.TryParse((string)jsonArg[nameof(BusinessProfileId)], out var businessProfileId))
            {
                BusinessProfileId = businessProfileId;
            }
            else
            {
                throw new ArgumentNullException(nameof(BusinessProfileId));
            }

            if (int.TryParse((string)jsonArg["BusinessProfiles_SearchEngineId"], out var businessProfilesSearchEngineId))
            {
                BusinessProfileSearchEngineId = businessProfilesSearchEngineId;
            }
            else
            {
                throw new ArgumentNullException(nameof(BusinessProfileSearchEngineId));
            }

            if (int.TryParse((string)jsonArg["UserId"], out var appDetailId))
            {
                UserId = appDetailId;
            }
            else
            {
                throw new ArgumentNullException(nameof(UserId));
            }

            if (jsonArg["Url"] != null)
            {
                Url2 = jsonArg["Url"].ToString();
            }
            else
            {
                throw new ArgumentNullException(nameof(Url2));
            }
        }

        public Report GenerateReportData(string Url, Device device)
        {
            var lightHouseResponse = _lightHouseHelper.Run(Url, device).GetAwaiter().GetResult();
            var audits = new List<Audit>();
            var categories = new List<Category>();
            var categoryGroups = new List<CategoryGroup>();
            var timings = new List<Timing>();

            foreach (var audit in lightHouseResponse.JsonData.Audits)
            {
                var formattedAudit = JsonConvert.DeserializeObject<Audit>(audit.Value.ToString());
                if (formattedAudit.Id != "is-on-https")
                {
                    audits.Add(formattedAudit);
                }
            }

            foreach (var cat in lightHouseResponse.JsonData.Categories)
            {
                var formattedCat = JsonConvert.DeserializeObject<Category>(cat.Value.ToString());
                categories.Add(formattedCat);
            }

            foreach (var catGroup in lightHouseResponse.JsonData.CategoryGroups)
            {
                var formattedcatGroup = JsonConvert.DeserializeObject<CategoryGroup>(catGroup.Value.ToString());
                categoryGroups.Add(formattedcatGroup);
            }

            timings = JsonConvert.DeserializeObject<List<Timing>>(lightHouseResponse.JsonData.Timing?.entries?.ToString());

            var report = new Report()
            {
                RequestedUrl = lightHouseResponse.JsonData?.RequestedUrl,
                FinalUrl = lightHouseResponse.JsonData?.FinalUrl,
                GatherMode = lightHouseResponse.JsonData?.GatherMode,
                UserAgent = lightHouseResponse.JsonData?.UserAgent,
                Device = Device.Mobile,
                MetaData = JsonConvert.SerializeObject(lightHouseResponse.JsonData),
                Audits = audits,
                Categories = categories,
                CategoryGroups = categoryGroups,
                Timings = timings,
                Environment = lightHouseResponse.JsonData.Environment,
                Performance = (int)(categories.Find(x => x.Id == "performance")?.Score.Value * 100),
                Accessibility = (int)(categories.Find(x => x.Id == "accessibility")?.Score.Value * 100),
                BestPractices = (int)(categories.Find(x => x.Id == "best-practices")?.Score.Value * 100),
                SEO = (int)(categories.Find(x => x.Id == "seo")?.Score.Value * 100),
                PWA = (int)(categories.Find(x => x.Id == "pwa")?.Score.Value * 100),
                BenchmarkIndex = lightHouseResponse.JsonData.Environment?.BenchmarkIndex.Value,
                FetchTime = lightHouseResponse.JsonData.FetchTime,
                HtmlReport = lightHouseResponse.HtmlReport
            };

            return report;
        }

        public void ProcessAllTasks()
        {
            var scheduledTasks = _taskRepository.Get()
                .Where(t => t.IsActive && t.StartTime != null && t.StartTime < DateTime.Now && (t.EndTime == null || (t.EndTime != null && t.EndTime >= DateTime.Now)))
                .OrderBy(t => t.Priority)
                .ToList();
            scheduledTasks.ForEach(task =>
            {
                try
                {
                    Console.WriteLine($"Processing taskId: {task.Id}, with Domain: {task.Domain}");

                    Console.WriteLine("Running Lighthouse 3 time(s)");
                    var report = GetCummulativeReport(task);

                    var emailModel = GetEmailModelData(task, report, true);
                    Console.WriteLine("Delta calculation...done.");

                    SendEmail(emailModel);
                    Console.WriteLine("Emails sending...done.");

                    SaveReportData(report);
                    Console.WriteLine("Report saving...done.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        private EmailNotificationModel GetEmailModelData(ScheduledTask task, Report CurrentReport, bool IsAttachmentRequired)
        {
            var user = _userRepository.Get(task.UserId);
            var totalDaySpan = (DateTime.UtcNow.Date - task.StartTime.Date).Days;
            var today = DateTime.UtcNow.Date;
            var deltaChanges = new Dictionary<LastReportType, DeltaChange>();

            var lastReports = _reportRepository.Get()
                                               .Where(r => r.TaskId == task.Id && r.FetchTime.Date >= DateTime.UtcNow.AddDays(-30).Date && r.FetchTime.Date < DateTime.UtcNow.Date)
                                               .OrderBy(r => r.FetchTime)
                                               .ToList();

            if (totalDaySpan >= 1 && lastReports.Find(r => r.FetchTime.Date >= DateTime.UtcNow.AddDays(-1).Date) != null)
            {
                deltaChanges[LastReportType.OneDay] = GetDeltaChange(CurrentReport, lastReports, LastReportType.OneDay);
            }
            if (totalDaySpan >= 7 && lastReports.Find(r => r.FetchTime.Date >= DateTime.UtcNow.AddDays(-7).Date) != null)
            {
                deltaChanges[LastReportType.SevenDays] = GetDeltaChange(CurrentReport, lastReports, LastReportType.SevenDays);
            }
            if (totalDaySpan >= 30 && lastReports.Find(r => r.FetchTime.Date >= DateTime.UtcNow.AddDays(-30).Date) != null)
            {
                deltaChanges[LastReportType.ThirtyDays] = GetDeltaChange(CurrentReport, lastReports, LastReportType.ThirtyDays);
            }

            var emailModel = new EmailNotificationModel()
            {
                Url = CurrentReport.FinalUrl,
                Device = CurrentReport.Device.ToString(),
                FetchTime = CurrentReport.FetchTime.ToString("dd-MMM-yyyy", CultureInfo.CurrentCulture),
                NumberOfRuns = 3,
                Performance = CurrentReport.Performance,
                Accessibility = CurrentReport.Accessibility,
                BestPractices = CurrentReport.BestPractices,
                SEO = CurrentReport.SEO,
                PWA = CurrentReport.PWA,
                User = user,
                BenchmarkIndex = CurrentReport.BenchmarkIndex,
                RedirectLink = "",
                DeltaChanges = deltaChanges
            };

            if (IsAttachmentRequired)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.DeserializeObject<string>(CurrentReport.HtmlReport));
                emailModel.AttachmentStream = new MemoryStream(byteArray);
                emailModel.AttachmentName = $"Report_{task.Domain}_{ DateTime.Now.ToString("dd-MMM-yyyy", CultureInfo.CurrentCulture) }.html";
            }

            return emailModel;
        }

        private DeltaChange GetDeltaChange(Report report, List<Report> lastReports, LastReportType lastReportType)
        {
            var requiredReports = lastReports.Where(r => r.FetchTime.Date >= DateTime.UtcNow.AddDays(-(int)lastReportType).Date);
            var deltaChange = new DeltaChange()
            {
                Performance = report.Performance - (int)requiredReports.Average(r => r.Performance),
                Accessibility = report.Accessibility - (int)requiredReports.Average(r => r.Accessibility),
                BestPractices = report.BestPractices - (int)requiredReports.Average(r => r.BestPractices),
                SEO = report.SEO - (int)requiredReports.Average(r => r.SEO),
                PWA = report.PWA - (int)requiredReports.Average(r => r.PWA),
                FetchTime = requiredReports.First().FetchTime
            };
            return deltaChange;
        }

        private Report GetCummulativeReport(ScheduledTask task)
        {
            var reportList = new List<Report>();

            reportList.Add(GenerateReportData(task.Url, Device.Desktop));
            Console.WriteLine("Run #1...done.");

            Thread.Sleep(1000);

            reportList.Add(GenerateReportData(task.Url, Device.Mobile));
            Console.WriteLine("Run #2...done.");

            Thread.Sleep(1000);

            reportList.Add(GenerateReportData(task.Url, Device.Mobile));
            Console.WriteLine("Run #3...done.");

            reportList[0].TaskId = task.Id;
            reportList[0].Performance = (int)reportList.Average(r => r.Performance);
            reportList[0].Accessibility = (int)reportList.Average(r => r.Accessibility);
            reportList[0].BestPractices = (int)reportList.Average(r => r.BestPractices);
            reportList[0].SEO = (int)reportList.Average(r => r.SEO);
            reportList[0].PWA = (int)reportList.Average(r => r?.PWA);
            reportList[0].BenchmarkIndex = (int)reportList.Average(r => r.BenchmarkIndex);

            return reportList[0];
        }

        private void SendEmail(EmailNotificationModel viewModel)
        {
            var templateParentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\RazorTemplates");
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(templateParentPath)
                                                      .Build();

            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = $"Website audit report generated on {viewModel.FetchTime} with the {viewModel.Device} device";
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress(_appSettings.EmailOptions.FromAddress);
                mailMessage.Body = engine.CompileRenderAsync("InvoiceEmailTemplate.cshtml", viewModel).Result;
                mailMessage.To.Add(new MailAddress(viewModel.User.Email));
                if (!string.IsNullOrEmpty(viewModel.AttachmentName) && viewModel.AttachmentStream != null)
                {
                    mailMessage.Attachments.Add(new Attachment(viewModel.AttachmentStream, viewModel.AttachmentName, "text/html"));
                }
                mailMessage.CC.Add(new MailAddress("sonu.k@deltax.com"));
                Console.WriteLine($"Mail send to Mr. {viewModel.User.FirstName}");
                _emailHelper.SendHTMLMail(mailMessage);
            }
        }
    }
}
