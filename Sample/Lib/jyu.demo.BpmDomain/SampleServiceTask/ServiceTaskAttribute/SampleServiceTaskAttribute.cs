namespace jyu.demo.BpmDomain.SampleServiceTask.ServiceTaskAttribute;

[AttributeUsage(AttributeTargets.Class)]
public class SampleServiceTaskAttribute : Attribute
{
    public SampleServiceTaskTopicName TopicName { get; }

    public SampleServiceTaskAttribute(
        SampleServiceTaskTopicName sampleServiceTaskTopicName
    )
    {
        TopicName = sampleServiceTaskTopicName;
    }
}