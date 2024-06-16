using System.Text.Json.Serialization;

namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class StartProcessRs
{
    [JsonPropertyName("id")] public string ProcessInstanceId { get; set; }
}