using System.Text.Json.Serialization;

namespace jyu.demo.Camunda.Models.CamundaEngineClient;

public class StartProcessInstanceRs
{
    [JsonPropertyName("id")]
    public string ProcessInstanceId { get; set; }
}