namespace jyu.demo.BpmDomain.SampleServiceTask.Enum;

using System.Runtime.Serialization;

public enum SampleServiceTaskTopicName
{
    [EnumMember(Value = "ServiceTask1")]
    ServiceTask1 = 1,

    [EnumMember(Value = "ServiceTask2")]
    ServiceTask2 = 2,
}