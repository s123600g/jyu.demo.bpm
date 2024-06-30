using jyu.demo.WorkerDomain.Works.SampleServiceTask.Models;

namespace jyu.demo.WorkerDomain.Works.SampleServiceTask.Interface;

public interface ISampleServiceTaskWorkBase
{
    /// <summary>
    /// 執行Work內容
    /// </summary>
    /// <param name="argSampleServiceTaskWorkData"></param>
    /// <returns></returns>
    Task ExecuteAsync(
        SampleServiceTaskWorkData argSampleServiceTaskWorkData
    );
}