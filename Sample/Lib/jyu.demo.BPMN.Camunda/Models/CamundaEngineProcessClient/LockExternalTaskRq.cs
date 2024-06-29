namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class LockExternalTaskRq
{
    public string WorkerId { get; set; }

    public int LockDuration { get; set; }
}