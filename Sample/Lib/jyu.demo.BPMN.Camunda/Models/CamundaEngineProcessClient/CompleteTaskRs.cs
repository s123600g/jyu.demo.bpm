namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class CompleteTaskRs
{
    public Dictionary<string, CompleteProcessCurrentTaskVariableDetail> Variables { get; set; }
}

public class CompleteProcessCurrentTaskVariableDetail
{
    public Object Value { get; set; }

    public string Type { get; set; }

    public Dictionary<string, Object> ValueInfo { get; set; }
}