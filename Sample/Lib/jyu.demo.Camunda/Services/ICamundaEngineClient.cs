using jyu.demo.Camunda.Models.CamundaEngineClient;

namespace jyu.demo.Camunda.Services;

public interface ICamundaEngineClient
{
    public Task<string?> StartNewProcessInstanceAsync(
        string processDefinitionKey
        , StartNewProcessInstanceRq startNewProcessInstanceRq
    );

    /// <summary>
    /// 查詢當前ProcessInstance Task資訊
    /// </summary>
    /// <param name="processInstanceId"></param>
    /// <returns></returns>
    public Task<List<QueryProcessCurrentTaskInfoRs>> QueryProcessCurrentTaskInfoAsync(
        string processInstanceId
    );

    /// <summary>
    /// 發送Complate Task請求
    /// </summary>
    /// <param name="processInstanceTaskId"></param>
    /// <param name="completeTaskRq"></param>
    /// <returns></returns>
    public Task<CompleteTaskRs> CompleteTaskAsync(
        string processInstanceTaskId
        , CompleteTaskRq completeTaskRq
    );

    /// <summary>
    /// 查詢當前需處理External Task清單
    /// </summary>
    /// <param name="queryExternalTaskRq"></param>
    /// <returns></returns>
    public Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync(
        QueryExternalTaskRq queryExternalTaskRq
    );

    /// <summary>
    /// 發送Lock External Task 請求
    /// </summary>
    /// <param name="externalTaskId"></param>
    /// <param name="argLockExternalTaskRq"></param>
    /// <returns></returns>
    public Task LockExternalTaskAsync(
        string externalTaskId
        , LockExternalTaskRq argLockExternalTaskRq
    );

    /// <summary>
    /// 發送Complete External Task
    /// </summary>
    /// <param name="externalTaskId"></param>
    /// <param name="complateExternalTaskRq"></param>
    /// <returns></returns>
    public Task ComplateExternalTaskAsync(
        string externalTaskId
        , ComplateExternalTaskRq complateExternalTaskRq
    );


    /// <summary>
    /// 查詢Process Instance攜帶變數
    /// </summary>
    /// <param name="processInstanceTaskId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T> QueryProcessInstanceVariable<T>(
        string processInstanceTaskId
    );

    /// <summary>
    /// 指派Task Assignee
    /// </summary>
    /// <param name="processInstanceTaskId"></param>
    /// <param name="assignee"></param>
    public Task SetTaskAssignee(
        string processInstanceTaskId
        , string assignee
    );
}