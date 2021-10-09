using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHost
{
    class LoggingConfiguration
    {
        private static Serilog.ILogger log => Serilog.Log.ForContext<LoggingConfiguration>();
        private static string outputTemplate =
    @"[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}Message: {Message}{NewLine}{Exception}{NewLine}";
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .CreateLogger();

            log.Debug("Hello");
        }
    }
}
