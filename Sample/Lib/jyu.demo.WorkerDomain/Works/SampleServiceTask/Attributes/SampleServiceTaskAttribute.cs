using jyu.demo.WorkerDomain.Works.SampleServiceTask.Enum;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask.Attributes;

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