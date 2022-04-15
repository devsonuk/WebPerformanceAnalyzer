using System.Threading.Tasks;

namespace WebPerformanceAnalyzerWorker.Helpers
{
    public  class WhereCmd : Terminal
    {
        protected override string FileName => "where.exe";
        internal async Task<string> GetNodePath()
        {
            return await this.Execute("node");
        }
    }
}
