namespace jyu.demo.Camunda.Models.CamundaEngineClient;

public class LockExternalTaskRq
{
    public string WorkerId { get; set; }

    public int LockDuration { get; set; }
}