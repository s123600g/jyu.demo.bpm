using jyu.demo.BPMN.Camunda.Models;
using jyu.demo.BPMN.Camunda.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;

namespace jyu.demo.BPMN;

public class LoadConfig
{
    private readonly string _defaultConfigFilePath;

    private readonly IConfiguration _config;
    private readonly ServiceCollection _services;

    public LoadConfig(
        string argConfigRootPath
    )
    {
        _defaultConfigFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            argConfigRootPath
        );

        _services = new ServiceCollection();
        _config = LoadConfigContent();
    }

    /// <summary>
    /// 取得IConfiguration 實體
    /// </summary>
    /// <returns></returns>
    public (IConfiguration config, ServiceProvider sp) GetConfiguration()
    {
        ServiceSetting();

        return (_config, _services.BuildServiceProvider());
    }

    /// <summary>
    /// NLog 載入初始化。
    /// </summary>
    /// <returns>回應NLog初始化後實體。</returns>
    public Logger NLogInitialize()
    {
        // NLog configuration with appsettings.json
        // https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json
        // 從組態設定檔載入NLog設定
        LogManager.Configuration = new NLogLoggingConfiguration(_config.GetSection("NLog"));
        Logger logger = LogManager.GetCurrentClassLogger();

        return logger;
    }

    #region 內部處理邏輯

    /// <summary>
    /// Service Setting
    /// </summary>
    private void ServiceSetting()
    {
        _services.Configure<CamundaConfigOptions>(_config.GetSection("CamundaSettings"));

        _services.AddHttpClient();
        _services.AddScoped<ICamundaEngineClient, CamundaEngineClient>();
    }

    /// <summary>
    /// 載入JSON組態設定檔
    /// </summary>
    private IConfiguration LoadConfigContent()
    {
        var configBuild = new ConfigurationBuilder()
                .SetBasePath(_defaultConfigFilePath)
            ;

        string[] getJsonFile = Directory.GetFiles(_defaultConfigFilePath);

        foreach (string item in getJsonFile)
        {
            configBuild.AddJsonFile(
                item,
                optional: true,
                reloadOnChange: false
            );
        }

        IConfiguration config = configBuild.Build();

        return config;
    }

    #endregion
}