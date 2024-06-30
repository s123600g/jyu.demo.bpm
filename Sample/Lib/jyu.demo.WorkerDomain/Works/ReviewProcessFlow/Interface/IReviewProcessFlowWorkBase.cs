using jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Models;

namespace jyu.demo.WorkerDomain.Works.ReviewProcessFlow.Interface;

public interface IReviewProcessFlowWorkBase
{
    /// <summary>
    /// 執行Work內容
    /// </summary>
    /// <param name="argReviewProcessFlowWorkData"></param>
    /// <returns></returns>
    Task ExecuteAsync(
        ReviewProcessFlowWorkData argReviewProcessFlowWorkData
    );
}