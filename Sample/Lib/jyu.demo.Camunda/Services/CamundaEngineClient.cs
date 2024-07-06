using System.Net.Http.Json;
using System.Text;
using jyu.demo.Camunda.Exceptions;
using jyu.demo.Camunda.Models;
using jyu.demo.Camunda.Models.CamundaEngineClient;
using jyu.demo.Common.Extension;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace jyu.demo.Camunda.Services;

public class CamundaEngineClient : ICamundaEngineClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly CamundaConfigOptions _camundaConfigOptions;
    private readonly string _camundaEngineBassAddress;

    public CamundaEngineClient(
        IHttpClientFactory httpClientFactory
        , IOptions<CamundaConfigOptions> options
    )
    {
        _httpClientFactory = httpClientFactory
                             ?? throw new ArgumentNullException(
                                 nameof(httpClientFactory)
                             );

        _camundaConfigOptions = options.Value;

        _camundaEngineBassAddress = GenCamundaEngineRootUrlPath();
    }

    /// <summary>
    /// 建置新 ProcessInstance
    /// </summary>
    /// <param name="processDefinitionKey"></param>
    /// <param name="startNewProcessInstanceRq"></param>
    /// <returns></returns>
    public async Task<string?> StartNewProcessInstanceAsync(
        string processDefinitionKey
        , StartNewProcessInstanceRq startNewProcessInstanceRq
    )
    {
        string path = $"process-definition/key/{processDefinitionKey}/start";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: startNewProcessInstanceRq
        );

        StartProcessInstanceRs? resContent = await httpRs.Content.ReadFromJsonAsync<StartProcessInstanceRs>();

        string? result = resContent?.ProcessInstanceId ?? null;

        return result;
    }

    public async Task<List<QueryProcessCurrentTaskInfoRs>> QueryProcessCurrentTaskInfoAsync(
        string processInstanceId
    )
    {
        string path = "task";

        Dictionary<string, string> queryParams = new Dictionary<string, string>
        {
            ["processInstanceId"] = processInstanceId
        };

        HttpResponseMessage httpRs = await HttpGetAsync(
            argPath: path
            , argQueryParams: queryParams
        );

        List<QueryProcessCurrentTaskInfoRs> result =
            await httpRs.Content.ReadFromJsonAsync<List<QueryProcessCurrentTaskInfoRs>>()
            ?? new List<QueryProcessCurrentTaskInfoRs>();

        return result;
    }

    public async Task<CompleteTaskRs> CompleteTaskAsync(
        string processInstanceTaskId
        , CompleteTaskRq completeTaskRq
    )
    {
        string path = $"task/{processInstanceTaskId}/complete";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: completeTaskRq
        );

        var resContent =
            await httpRs.Content.ReadFromJsonAsync<Dictionary<string, CompleteProcessCurrentTaskVariableDetail>>();

        CompleteTaskRs result = new CompleteTaskRs();

        if (
            resContent != null
        )
        {
            result.Variables = resContent;
        }

        return result;
    }

    public async Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync(
        QueryExternalTaskRq queryExternalTaskRq
    )
    {
        string path = $"external-task";

        Dictionary<string, string> queryParams = new Dictionary<string, string>
        {
            ["notLocked"] = queryExternalTaskRq.NotLocked.GetEnumMemberAttributeValue(),
            ["processDefinitionId"] = queryExternalTaskRq.ProcessDefinitionId,
        };

        HttpResponseMessage httpRs = await HttpGetAsync(
            argPath: path,
            argQueryParams: queryParams
        );

        List<QueryExternalTaskRs> result = await httpRs.Content.ReadFromJsonAsync<List<QueryExternalTaskRs>>()
                                           ?? new List<QueryExternalTaskRs>();

        return result;
    }

    public async Task LockExternalTaskAsync(
        string externalTaskId
        , LockExternalTaskRq argLockExternalTaskRq
    )
    {
        string path = $"external-task/{externalTaskId}/lock";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: argLockExternalTaskRq
        );

        if (
            !httpRs.IsSuccessStatusCode
        )
        {
            throw new CamundaApiFailException();
        }
    }

    public async Task ComplateExternalTaskAsync(
        string externalTaskId
        , ComplateExternalTaskRq complateExternalTaskRq
    )
    {
        string path = $"external-task/{externalTaskId}/complete";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: complateExternalTaskRq
        );

        if (
            !httpRs.IsSuccessStatusCode
        )
        {
            throw new CamundaApiFailException();
        }
    }

    public async Task<T> QueryProcessInstanceVariable<T>(
        string processInstanceTaskId
    )
    {
        string path = $"process-instance/{processInstanceTaskId}/variables";

        HttpResponseMessage httpRs = await HttpGetAsync(
            argPath: path
        );

        T result = await httpRs.Content.ReadFromJsonAsync<T>();

        return result;
    }


    public async Task SetTaskAssignee(
        string processInstanceTaskId
        , string assignee
    )
    {
        string path = $"task/{processInstanceTaskId}/assignee";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: new
            {
                UserId = assignee
            }
        );

        if (
            !httpRs.IsSuccessStatusCode
        )
        {
            throw new CamundaApiFailException();
        }
    }

    #region 內部處理邏輯

    private async Task<HttpResponseMessage> HttpGetAsync(
        string argPath
        , Dictionary<string, string>? argQueryParams = null
    )
    {
        using (var client = _httpClientFactory.CreateClient())
        {
            client.BaseAddress = new Uri(_camundaEngineBassAddress);
            string path = $"{_camundaConfigOptions.ContextPath}/{argPath}";

            if (
                argQueryParams != null
                &&
                argQueryParams.Any()
            )
            {
                path = QueryHelpers.AddQueryString(path, argQueryParams);
            }

            HttpResponseMessage httpRs = await client.GetAsync(requestUri: path);

            if (
                !httpRs.IsSuccessStatusCode
            )
            {
                throw new CamundaApiFailException();
            }

            return httpRs;
        }
    }

    private async Task<HttpResponseMessage> HttpPostAsJsonAsync(
        string argPath
        , object argRequestBody
    )
    {
        using (var client = _httpClientFactory.CreateClient())
        {
            client.BaseAddress = new Uri(_camundaEngineBassAddress);
            string path = $"{_camundaConfigOptions.ContextPath}/{argPath}";

            HttpResponseMessage httpRs = await client.PostAsJsonAsync(
                requestUri: path
                , value: argRequestBody
            );

            if (!httpRs.IsSuccessStatusCode)
            {
                throw new CamundaApiFailException();
            }

            return httpRs;
        }
    }

    /// <summary>
    /// 產生Camunda 流程引擎 API Root Url
    /// </summary>
    /// <returns></returns>
    private string GenCamundaEngineRootUrlPath()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append($"{_camundaConfigOptions.Protocol}://");
        stringBuilder.Append($"{_camundaConfigOptions.Host}");

        if (_camundaConfigOptions.Port.HasValue)
        {
            stringBuilder.Append($":{_camundaConfigOptions.Port}/");
        }
        else
        {
            stringBuilder.Append("/");
        }

        stringBuilder.Append(_camundaConfigOptions.ContextPath);

        return stringBuilder.ToString();
    }

    #endregion
}