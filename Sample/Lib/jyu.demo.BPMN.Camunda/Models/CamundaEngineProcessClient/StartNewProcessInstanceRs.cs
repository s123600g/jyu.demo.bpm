using System.Text.Json.Serialization;

namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

public class StartProcessInstanceRs
{
    [JsonPropertyName("id")]
    public string ProcessInstanceId { get; set; }
}