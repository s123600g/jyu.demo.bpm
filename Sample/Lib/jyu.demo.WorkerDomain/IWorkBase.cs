using jyu.demo.WorkerDomain.Models;

namespace jyu.demo.WorkerDomain;

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