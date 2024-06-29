using jyu.demo.Camunda.Models.CamundaEngineClient;

namespace jyu.demo.BPMN.Services;

public interface IBpmnFlow
{
    public Task Execute(
        StartNewProcessInstanceRq argStartNewProcessInstancePayload
    );
}