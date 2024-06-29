using jyu.demo.BPMN;
using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.BPMN.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

internal class Program
{
    static async Task Main(string[] args)
    {
        Logger? log = null;
        IServiceProvider? sp = null;
        IConfiguration? ap = null;

        #region App init

        LoadConfig loadConfig = new LoadConfig(argConfigRootPath: "Config");

        (IConfiguration config, ServiceProvider sp) configEntity = loadConfig.GetConfiguration();

        // 載入DI服務容器實體
        sp = configEntity.sp;

        // 載入appsettings.json內容配置
        ap = configEntity.config;

        // 載入NLog初始化
        log = loadConfig.NLogInitialize();

        #endregion

        try
        {
            ICamundaEngineClient camundaEngineClient = sp.GetService<ICamundaEngineClient>() ??
                                                       throw new ArgumentNullException(
                                                           nameof(ICamundaEngineClient)
                                                       );

            IBpmnFlow creditCardPaymentBpmnFlow = new CreditCardPaymentBpmnFlow1(
                argCmundaEngineClient: camundaEngineClient
                , argLogger: log
            );

            await creditCardPaymentBpmnFlow.Execute(
                argStartNewProcessInstancePayload: new StartNewProcessInstanceRq
                {
                    Variables = new Dictionary<string, StartNewProcessInstanceVariablesDetail>
                    {
                        {
                            "amount", new StartNewProcessInstanceVariablesDetail()
                            {
                                Value = 1000, Type = "string"
                            }
                        }
                    }
                }
            );
        }
        catch (Exception ex)
        {
            log.Error(
                message: ex.Message
                , exception: ex.InnerException
            );

            throw;
        }

        log.Info("Done.");
    }
}