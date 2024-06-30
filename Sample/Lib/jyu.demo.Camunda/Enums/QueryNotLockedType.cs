using System.Runtime.Serialization;

namespace jyu.demo.Camunda.Enums;

public enum QueryNotLockedType
{
    [EnumMember(Value ="true")]
    Lock = 1,
    
    [EnumMember(Value ="false")]
    NoLock = 2,
}