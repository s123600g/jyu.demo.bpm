namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class CompleteTaskRq
{
    public Dictionary<string, CompleteProcessCurrentTaskVariablesDetail> Variables { get; set; }

    public bool WithVariablesInReturn { get; set; }
}

public class CompleteProcessCurrentTaskVariablesDetail
{
    public Object Value { get; set; }
}