namespace jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;

using System.Text.Json.Serialization;

public class QueryExternalTaskRs
{
    [JsonPropertyName("id")]
    public string ExternalTaskId { get; set; }

    [JsonPropertyName("workerId")]
    public string WorkerId { get; set; }

    [JsonPropertyName("topicName")]
    public string TopicName { get; set; }

    [JsonPropertyName("processInstanceId")]
    public string ProcessInstanceId { get; set; }
}