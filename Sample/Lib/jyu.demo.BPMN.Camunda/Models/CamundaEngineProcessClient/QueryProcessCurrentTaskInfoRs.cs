using System.Text.Json.Serialization;

namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class QueryProcessCurrentTaskInfoRs
{
    [JsonPropertyName("id")] public string TaskId { get; set; }

    [JsonPropertyName("name")] public string TaskName { get; set; }

    [JsonPropertyName("assignee")] public string TaskAssignee { get; set; }

    [JsonPropertyName("processInstanceId")]
    public string ProcessInstanceId { get; set; }
}