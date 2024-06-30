using System.Runtime.Serialization;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;

public enum ReviewProcessFlowTopicName
{
    [EnumMember(Value = "AssignApprovalCheckpoint")]
    AssignApprovalCheckpoint = 1,
    
    [EnumMember(Value = "ProcessApprovalResults")]
    ProcessApprovalResults = 2,
}