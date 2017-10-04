using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Core
{
    public static class Flogger
    {
        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Roulette",
                "logs");

        static Flogger()
        {
            

            _perfLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(path, "perf.txt"))
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(path, "usage.txt"))
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(path, "error.txt"))
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(path, "diagnostic.txt"))
                .CreateLogger();
        }

        public static void WritePerf(FlogDetail infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteUsage(FlogDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteError(FlogDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteDiagnostic(FlogDetail infoToLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            if (!writeDiagnostics)
                return;

            _diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string FindProcName(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty(procName))
                    return procName;
            }

            if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
            {
                return (string)ex.Data["Procedure"];
            }

            if (ex.InnerException != null)
                return FindProcName(ex.InnerException);

            return null;
        }

        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }

        public static FlogDetail GetFlogDetail(string message, Exception ex)
        {
            const string Product = "Flogger";
            const string Location = "FloggerConsole"; // this application
            const string Layer = "Job"; // unattended executable invoked somehow
            var user = Environment.UserName;
            var hostname = Environment.MachineName;

            return new FlogDetail
            {
                Product = Product,
                Location = Location,
                Layer = Layer,
                UserName = user,
                Hostname = hostname,
                Message = message,
                Exception = ex
            };
        }
    }
}
