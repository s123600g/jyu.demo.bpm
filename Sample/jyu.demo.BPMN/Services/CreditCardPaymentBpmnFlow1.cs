using jyu.demo.BPMN.Camunda.Exceptions;
using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;
using jyu.demo.BPMN.Camunda.Services;
using NLog;

namespace jyu.demo.BPMN.Services;

public class CreditCardPaymentBpmnFlow1 : IBpmnFlow
{
    private readonly ICamundaEngineClient _camundaEngineClient;
    private readonly Logger _log;

    private readonly string _processDefinitionKey;

    public CreditCardPaymentBpmnFlow1(
        ICamundaEngineClient argCmundaEngineClient
        , Logger argLogger
    )
    {
        _camundaEngineClient = argCmundaEngineClient ?? throw new ArgumentNullException(nameof(argCmundaEngineClient));
        _log = argLogger ?? throw new ArgumentNullException(nameof(argLogger));

        _processDefinitionKey = "CreditCardPayment_v1";
    }

    private string _processInstanceId { get; set; } = string.Empty;

    private string _currentTaskId { get; set; } = string.Empty;

    public async Task Execute(
        StartNewProcessInstanceRq argStartNewProcessInstancePayload
    )
    {
        /*
         * 收到商家請款請求
         * Step 1. 建立新 Process Instance
         */
        _processInstanceId =
            await _camundaEngineClient.StartNewProcessInstanceAsync(
                argProcessDefinitionKey: _processDefinitionKey
                , argStartNewProcessInstanceRq: argStartNewProcessInstancePayload
            ) ?? throw new NullReferenceException("ProcessInstanceId is null.");

        _log.Info($"New process instance id：{_processInstanceId}");
        
        List<QueryProcessCurrentTaskInfoRs> taskInfo = await _camundaEngineClient.QueryProcessCurrentTaskInfoAsync(
            argProcessInstanceId: _processInstanceId
        );

        _currentTaskId = taskInfo.FirstOrDefault()?.TaskId ?? throw new TaskInfoIsNullException();

        _log.Info($"Current process instance task id：{_currentTaskId}");
        
        /*
         * 系統執行請款支付處理
         *         .
         *         .
         *         .
         * 處理完成通知請款商家
         */
        
        /*
         * 商家收到請款成功訊息
         * 商家處理商品後續
         * 已完成
         * Step 2. 當前User Task(請款商家)任務已完成
         */
        
    }
}