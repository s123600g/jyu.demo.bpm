using jyu.demo.Camunda.Enums;

namespace jyu.demo.Camunda.Models.CamundaEngineClient;

public class QueryExternalTaskRq
{
    public QueryNotLockedType NotLocked { get; set; }

    public string ProcessDefinitionId { get; set; }
}