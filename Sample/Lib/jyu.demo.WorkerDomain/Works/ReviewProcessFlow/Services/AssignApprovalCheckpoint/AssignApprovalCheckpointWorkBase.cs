using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.SampleDb.Dal;
using jyu.demo.SampleDb.Dao;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.AssignApprovalCheckpoint.Models;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Attributes;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.AssignApprovalCheckpoint;

[ReviewProcessFlow(ReviewProcessFlowTopicName.AssignApprovalCheckpoint)]
public class AssignApprovalCheckpointWorkBase : IReviewProcessFlowWorkBase
{
    private readonly SampleDbContext _db;
    private readonly ICamundaEngineClient _camundaEngineClient;

    private readonly string _workerId;
    private readonly int _lockDuration;

    public AssignApprovalCheckpointWorkBase(
        SampleDbContext sampleDbContext
        , ICamundaEngineClient camundaEngineClient
    )
    {
        _db = sampleDbContext
              ?? throw new ArgumentNullException(
                  nameof(sampleDbContext)
              );

        _camundaEngineClient = camundaEngineClient
                               ?? throw new ArgumentNullException(
                                   nameof(camundaEngineClient)
                               );

        _workerId = ReviewProcessFlowTopicName.AssignApprovalCheckpoint.GetEnumMemberAttributeValue();

        _lockDuration = 100000;
    }

    public async Task ExecuteAsync(
        ReviewProcessFlowWorkData argReviewProcessFlowWorkData
    )
    {
        // 查詢當前ProcessInstance 包含Variable
        ProductReviewVariable variable = await _camundaEngineClient.QueryProcessInstanceVariable<ProductReviewVariable>(
            argProcessInstanceTaskId: argReviewProcessFlowWorkData.ProcessInstanceId
        );

        // 執行 Lock external task
        await _camundaEngineClient.LockExternalTaskAsync(
            argExternalTaskId: argReviewProcessFlowWorkData.ExternalTaskId
            , argLockExternalTaskRq: new LockExternalTaskRq
            {
                WorkerId = _workerId,
                LockDuration = _lockDuration,
            }
        );

        #region 主要處理區塊

        string reviewer = string.Empty;

        if (
            variable.Price.Value <= 100
        )
        {
            reviewer = "A";
        }
        else
        {
            reviewer = "B";
        }

        string id = Guid.NewGuid().ToString();

        await _db.AddAsync(new ProductApplicationHistory
        {
            Id = id,
            ProductName = variable.ProductName.Value,
            Price = variable.Price.Value,
            Status = (int)ReviewStatusType.PendingReview,
            Reviewer = reviewer,
            ProcessInstanceId = argReviewProcessFlowWorkData.ProcessInstanceId
        });

        #endregion

        // Complate external task
        await _camundaEngineClient.ComplateExternalTaskAsync(
            argExternalTaskId: argReviewProcessFlowWorkData.ExternalTaskId
            , argComplateExternalTaskRq: new ComplateExternalTaskRq
            {
                WorkerId = _workerId,
                Variables = new AssignApprovalCheckpointWorkCompleteVariable
                {
                    Id = new VariableDetail<string>
                    {
                        Value = id
                    },

                    Reviewer = new VariableDetail<string>
                    {
                        Value = reviewer
                    },
                }
            }
        );

        await _db.SaveChangesAsync();
    }
}