using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Camunda.Services;
using jyu.demo.Common.Extension;
using jyu.demo.SampleDb.Dal;
using jyu.demo.SampleDb.Exceptions;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Attributes;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Enums;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;
using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.ProcessApprovalResults;

[ReviewProcessFlow(ReviewProcessFlowTopicName.ProcessApprovalResults)]
public class ProcessApprovalResultsWorkBase : IReviewProcessFlowWorkBase
{
    private readonly SampleDbContext _db;
    private readonly ICamundaEngineClient _camundaEngineClient;

    private readonly string _workerId;
    private readonly int _lockDuration;

    public ProcessApprovalResultsWorkBase(
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

        _workerId = ReviewProcessFlowTopicName.ProcessApprovalResults.GetEnumMemberAttributeValue();

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

        #region 主要處理區塊

        var data = await _db.ProductApplicationHistories.Where(item =>
            item.Id == variable.Id.Value
        ).FirstOrDefaultAsync() ?? throw new DataNotFoundException();

        if (
            variable.IsApproval.Value
        )
        {
            data.Status = (int)ReviewStatusType.Approval;
        }
        else
        {
            data.Status = (int)ReviewStatusType.Reject;
        }

        #endregion

        // 執行 Lock external task
        await _camundaEngineClient.LockExternalTaskAsync(
            argExternalTaskId: argReviewProcessFlowWorkData.ExternalTaskId
            , argLockExternalTaskRq: new LockExternalTaskRq
            {
                WorkerId = _workerId,
                LockDuration = _lockDuration,
            }
        );

        // Complate external task
        await _camundaEngineClient.ComplateExternalTaskAsync(
            argExternalTaskId: argReviewProcessFlowWorkData.ExternalTaskId
            , argComplateExternalTaskRq: new ComplateExternalTaskRq
            {
                WorkerId = _workerId,
            }
        );

        await _db.SaveChangesAsync();
    }
}