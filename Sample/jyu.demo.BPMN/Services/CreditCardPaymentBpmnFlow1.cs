using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;
using jyu.demo.BPMN.Camunda.Services.BpmEngingClient;
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

    private string _processInstanceId { get; set; }

    private string _currentTaskId { get; set; }

    public async Task Execute(
        StartNewProcessInstanceRq argStartNewProcessInstancePayload
    )
    {
        // Step 1. Start new process instance
        _processInstanceId =
            await _camundaEngineClient.StartNewProcessInstanceAsync(
                argProcessDefinitionKey: _processDefinitionKey
                , argStartNewProcessInstanceRq: argStartNewProcessInstancePayload
            ) ?? throw new NullReferenceException("ProcessInstanceId is null.");

        _log.Info($"New process instance id：{_processInstanceId}");

        // Step 2. Complete user task for billing vendor
    }
}