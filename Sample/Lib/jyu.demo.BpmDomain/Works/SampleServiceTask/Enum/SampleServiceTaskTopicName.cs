using System.Runtime.Serialization;

namespace jyu.demo.BpmDomain.Works.SampleServiceTask.Enum;

public enum SampleServiceTaskTopicName
{
    [EnumMember(Value = "ServiceTask1")]
    ServiceTask1 = 1,

    [EnumMember(Value = "ServiceTask2")]
    ServiceTask2 = 2,
}