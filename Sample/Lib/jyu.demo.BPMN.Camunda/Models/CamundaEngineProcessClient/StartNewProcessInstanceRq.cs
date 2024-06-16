namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class StartNewProcessInstanceRq
{
    public Dictionary<string, StartNewProcessInstanceVariablesDetail> Variables { get; set; }
}

public class StartNewProcessInstanceVariablesDetail
{
    public Object Value { get; set; }

    public string Type { get; set; }
}