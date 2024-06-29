namespace jyu.demo.BpmDomain.Works.SampleServiceTask.Attributes;

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