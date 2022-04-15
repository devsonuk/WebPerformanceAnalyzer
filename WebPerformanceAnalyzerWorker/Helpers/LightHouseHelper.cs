using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebPerformanceAnalyzerWorker.Entities;
using WebPerformanceAnalyzerWorker.Enums;
using WebPerformanceAnalyzerWorker.Objects;
using static WebPerformanceAnalyzerWorker.Objects.AuditRequest;

namespace WebPerformanceAnalyzerWorker.Helpers
{
    public class LightHouseHelper
    {
        public async Task<LightHouseResponse> Run(string urlWithProtocol, Device deviceType)
        {
            var auditRequest = new AuditRequest(urlWithProtocol);

            if (deviceType == Device.Mobile)
            {
                auditRequest.EmulatedFormFactor = FormFactor.Mobile;
            }
            return await Run(auditRequest).ConfigureAwait(false);
        }
        public Task<LightHouseResponse> Run(AuditRequest request)
        {
            request.EnableLogging = true;
            if (request == null) throw new ArgumentNullException(nameof(request));
            return RunAfterCheck(request);
        }
        private async Task<LightHouseResponse> RunAfterCheck(AuditRequest request)
        {
            var cmd = new WhereCmd()
            {
                EnableDebugging = request.EnableLogging
            };
            var nodePath = await cmd.GetNodePath().ConfigureAwait(false);
            if (String.IsNullOrEmpty(nodePath) || !File.Exists(nodePath)) throw new Exception("Couldn't find NodeJs. Please, install NodeJs and make sure than PATH variable defined.");

            var npm = new Npm(nodePath)
            {
                EnableDebugging = request.EnableLogging
            };
            var npmPath = await npm.GetNpmPath().ConfigureAwait(false);

            var version = await npm.GetLighthouseVersion().ConfigureAwait(false);

            var sm = new ScriptMaker();
            var content = sm.Produce(request, npmPath, version);
            if (!sm.Save(content)) throw new Exception($"Couldn't save JS script to %temp% directory. Path: {sm.TempFileName}");

            try
            {
                var node = new Node()
                {
                    EnableDebugging = request.EnableLogging
                };
                var stdoutJson = await node.Run(sm.TempFileName).ConfigureAwait(false);
                var dataObj = JObject.Parse(stdoutJson);
                var lightHouseResponse = new LightHouseResponse();

                if (dataObj["lhr"] != null)
                {
                    lightHouseResponse.JsonData = JsonConvert.DeserializeObject<JsonData>(dataObj["lhr"].ToString());
                }

                if (dataObj["report"] != null)
                {
                    lightHouseResponse.HtmlReport = JsonConvert.SerializeObject(dataObj["report"]);
                }
                
                return lightHouseResponse;
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(ex.Message) && Regex.IsMatch(ex.Message, @"Cannot find module[\s\S]+?node_modules\\lighthouse'"))
                {
                    throw new Exception("Lighthouse is not installed. Please, execute `npm install -g lighthouse` in console.");
                }
                throw;
            }
            finally
            {
                if (!npm.EnableDebugging) sm.Delete();
            }
        }
    }
}
