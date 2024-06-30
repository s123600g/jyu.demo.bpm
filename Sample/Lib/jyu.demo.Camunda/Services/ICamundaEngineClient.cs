using jyu.demo.Camunda.Models.CamundaEngineClient;

namespace jyu.demo.Camunda.Services;

public interface ICamundaEngineClient
{
    public Task<string?> StartNewProcessInstanceAsync(
        string argProcessDefinitionKey
        , StartNewProcessInstanceRq argStartNewProcessInstanceRq
    );

    /// <summary>
    /// 查詢當前ProcessInstance Task資訊
    /// </summary>
    /// <param name="argProcessInstanceId"></param>
    /// <returns></returns>
    public Task<List<QueryProcessCurrentTaskInfoRs>> QueryProcessCurrentTaskInfoAsync(
        string argProcessInstanceId
    );

    /// <summary>
    /// 發送Complate Task請求
    /// </summary>
    /// <param name="argProcessInstanceTaskId"></param>
    /// <param name="argCompleteTaskRq"></param>
    /// <returns></returns>
    public Task<CompleteTaskRs> CompleteTaskAsync(
        string argProcessInstanceTaskId
        , CompleteTaskRq argCompleteTaskRq
    );

    /// <summary>
    /// 查詢當前需處理External Task清單
    /// </summary>
    /// <param name="argQueryExternalTaskRq"></param>
    /// <returns></returns>
    public Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync(
        QueryExternalTaskRq argQueryExternalTaskRq
    );

    /// <summary>
    /// 發送Lock External Task 請求
    /// </summary>
    /// <param name="argExternalTaskId"></param>
    /// <param name="argLockExternalTaskRq"></param>
    /// <returns></returns>
    public Task LockExternalTaskAsync(
        string argExternalTaskId
        , LockExternalTaskRq argLockExternalTaskRq
    );

    /// <summary>
    /// 發送Complete External Task
    /// </summary>
    /// <param name="argExternalTaskId"></param>
    /// <param name="argComplateExternalTaskRq"></param>
    /// <returns></returns>
    public Task ComplateExternalTaskAsync(
        string argExternalTaskId
        , ComplateExternalTaskRq argComplateExternalTaskRq
    );


    /// <summary>
    /// 查詢Process Instance攜帶變數
    /// </summary>
    /// <param name="argProcessInstanceTaskId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T> QueryProcessInstanceVariable<T>(
        string argProcessInstanceTaskId
    );
}