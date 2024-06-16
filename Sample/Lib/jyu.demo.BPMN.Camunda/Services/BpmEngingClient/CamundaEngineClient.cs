using System.Net.Http.Json;
using System.Text;
using jyu.demo.BPMN.Camunda.Exceptions;
using jyu.demo.BPMN.Camunda.Models;
using jyu.demo.BPMN.Camunda.Models.CamundaEngineProcessClient;
using Microsoft.Extensions.Options;

namespace jyu.demo.BPMN.Camunda.Services.BpmEngingClient;

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
        _camundaConfigOptions = argCamundaConfigOptions.Value;

        _camundaEngineBassAddress = GenCamundaEngineRootUrlPath();
        _httpClientFactory = argHttpClientFactory;
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

        HttpResponseMessage httpRs =
            await HttpPostAsJsonAsync(argPath: path, argRequestBody: argStartNewProcessInstanceRq);

        StartProcessRs? resContent = await httpRs.Content.ReadFromJsonAsync<StartProcessRs>();

        string? result = resContent?.ProcessInstanceId ?? null;

        return result;
    }

    public async Task<string?> QueryProcessCurrentTaskIdAsync(
        string argProcessInstanceId
    )
    {
        string result = null;

        using (var client = _httpClientFactory.CreateClient())
        {
            client.BaseAddress = new Uri(_camundaEngineBassAddress);

            string path = $"";
        }

        return result;
    }

    public string CompleteProcessCurrentTaskAsync(
        string argProcessInstanceTaskId
    )
    {
        throw new NotImplementedException();
    }

    #region 內部處理邏輯

    private async Task<HttpResponseMessage> HttpPostAsJsonAsync(
        string argPath
        , object argRequestBody
    )
    {
        HttpResponseMessage httpRs = new HttpResponseMessage();

        using (var client = _httpClientFactory.CreateClient())
        {
            client.BaseAddress = new Uri(_camundaEngineBassAddress);
            string path = $"{_camundaConfigOptions.ContextPath}/{argPath}";

            httpRs = await client.PostAsJsonAsync(path, argRequestBody);

            if (!httpRs.IsSuccessStatusCode)
            {
                throw new CamundaApiFailException();
            }
        }

        return httpRs;
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