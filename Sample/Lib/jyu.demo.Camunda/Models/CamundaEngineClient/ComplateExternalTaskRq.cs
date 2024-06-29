namespace jyu.demo.Camunda.Models.CamundaEngineClient;

public class ComplateExternalTaskRq
{
    public string WorkerId { get; set; }

    public Dictionary<string, object> Variables { get; set; }

    public Dictionary<string, object> LocalVariables { get; set; }
}