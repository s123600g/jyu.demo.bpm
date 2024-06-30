using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.AssignApprovalCheckpoint.Models;

public class AssignApprovalCheckpointWorkCompleteVariable
{
    public VariableDetail<string> Id { get; set; }
    
    public VariableDetail<string> Reviewer { get; set; }
}