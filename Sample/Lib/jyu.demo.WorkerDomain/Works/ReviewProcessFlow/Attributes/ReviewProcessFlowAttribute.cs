using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ReviewProcessFlowAttribute : Attribute
{
    public ReviewProcessFlowTopicName TopicName { get; }

    public ReviewProcessFlowAttribute(
        ReviewProcessFlowTopicName reviewProcessFlowTopicName
    )
    {
        TopicName = reviewProcessFlowTopicName;
    }
}