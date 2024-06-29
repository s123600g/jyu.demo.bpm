using System.Net.Http.Json;
using System.Text;
using jyu.demo.Camunda.Exceptions;
using jyu.demo.Camunda.Models;
using jyu.demo.Camunda.Models.CamundaEngineClient;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace jyu.demo.Camunda.Services;

public class CamundaEngineClient : ICamundaEngineClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly CamundaConfigOptions _camundaConfigOptions;
    private readonly string _camundaEngineBassAddress;

    public CamundaEngineClient(
        IHttpClientFactory argHttpClientFactory
        , IOptions<CamundaConfigOptions> argCamundaConfigOptions
    )
    {
        _httpClientFactory = argHttpClientFactory
                             ?? throw new ArgumentNullException(
                                 nameof(argHttpClientFactory)
                             );

        _camundaConfigOptions = argCamundaConfigOptions.Value;

        _camundaEngineBassAddress = GenCamundaEngineRootUrlPath();
    }

    /// <summary>
    /// 建置新 ProcessInstance
    /// </summary>
    /// <param name="argProcessDefinitionKey"></param>
    /// <param name="argStartNewProcessInstanceRq"></param>
    /// <returns></returns>
    public async Task<string?> StartNewProcessInstanceAsync(
        string argProcessDefinitionKey
        , StartNewProcessInstanceRq argStartNewProcessInstanceRq
    )
    {
        string path = $"process-definition/key/{argProcessDefinitionKey}/start";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: argStartNewProcessInstanceRq
        );

        StartProcessInstanceRs? resContent = await httpRs.Content.ReadFromJsonAsync<StartProcessInstanceRs>();

        string? result = resContent?.ProcessInstanceId ?? null;

        return result;
    }

    public async Task<List<QueryProcessCurrentTaskInfoRs>> QueryProcessCurrentTaskInfoAsync(
        string argProcessInstanceId
    )
    {
        string path = "task";

        Dictionary<string, string> queryParams = new Dictionary<string, string>
        {
            ["processInstanceId"] = argProcessInstanceId
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
        string argProcessInstanceTaskId
        , CompleteTaskRq argCompleteTaskRq
    )
    {
        string path = $"task/{argProcessInstanceTaskId}/complete";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: argCompleteTaskRq
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

    public async Task<List<QueryExternalTaskRs>> QueryExternalTaskAsync()
    {
        string path = $"external-task";

        HttpResponseMessage httpRs = await HttpGetAsync(
            argPath: path
        );

        List<QueryExternalTaskRs> result = await httpRs.Content.ReadFromJsonAsync<List<QueryExternalTaskRs>>()
                                           ?? new List<QueryExternalTaskRs>();

        return result;
    }

    public async Task LockExternalTaskAsync(
        string argExternalTaskId
        , LockExternalTaskRq argLockExternalTaskRq
    )
    {
        string path = $"external-task/{argExternalTaskId}/lock";

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
        string argExternalTaskId
        , ComplateExternalTaskRq argComplateExternalTaskRq
    )
    {
        string path = $"external-task/{argExternalTaskId}/complete";

        HttpResponseMessage httpRs = await HttpPostAsJsonAsync(
            argPath: path
            , argRequestBody: argComplateExternalTaskRq
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