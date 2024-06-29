using jyu.demo.BpmDomain.Models;

namespace jyu.demo.SampleServiceTaskWorker.Services;

public interface IWorkBase
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