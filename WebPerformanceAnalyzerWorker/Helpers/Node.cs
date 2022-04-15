using System;
using System.Threading.Tasks;
using WebPerformanceAnalyzerWorker.Helpers;

namespace WebPerformanceAnalyzerWorker.Helpers
{
    public class Node : Terminal
    {
        protected override string FileName => "node";
        public async Task<string> Run(string jsFilePath)
        {
            return await this.Execute("--harmony --unhandled-rejections=strict " + jsFilePath).ConfigureAwait(false);
        }
        protected override void OnError(string message)
        {
            throw new Exception(message);
        }
    }

}
