using jyu.demo.BpmDomain.SampleServiceTask.Models;

namespace jyu.demo.SampleServiceTaskWorker.Services;

public interface IServiceTaskWork
{
    /// <summary>
    /// 執行Work內容
    /// </summary>
    /// <param name="argServiceTaskWorkData"></param>
    /// <returns></returns>
    Task ExecuteAsync(
        ServiceTaskWorkData argServiceTaskWorkData
    );
}