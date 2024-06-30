namespace jyu.demo.Camunda.Models.CamundaEngineClient;

public class ComplateExternalTaskRq
{
    public string WorkerId { get; set; }

    public object Variables { get; set; }
}