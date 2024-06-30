using jyu.demo.Camunda.Models;
using jyu.demo.SampleDb.Dal;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

namespace jyu.demo.ReviewProcessFlowWorker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var config = builder.Configuration;
        var log = builder.Logging;
        var services = builder.Services;

        NLogLoggingConfiguration nlogConfig = new NLogLoggingConfiguration(
            config.GetSection("NLog")
        );
        log.AddNLog(nlogConfig);

        services.AddDbContext<SampleDbContext>(opt =>
        {
            var dbConnStr = config.GetConnectionString(name: "DefaultConnection");

            if (
                string.IsNullOrEmpty(dbConnStr)
            )
            {
                throw new ArgumentNullException(nameof(dbConnStr));
            }

            opt.UseSqlite(
                connectionString: dbConnStr
            );
        });

        services.Configure<CamundaConfigOptions>(
            config.GetSection("CamundaSettings")
        );

        services.Configure<ProcessDefinitionOptions>(
            config.GetSection("ProcessDefinitions")
        );

        services.AddHttpClient();

        services.AddWorkerRelatedServices();

        // Work Instance注入
        services.AddHostedService<ReviewProcessFlowWorker>();

        var host = builder.Build();
        host.Run();
    }
}