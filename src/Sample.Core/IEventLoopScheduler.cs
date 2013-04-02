using System;
using System.Reactive.Concurrency;

namespace Sample.Core
{
    /// <summary>
    /// Interface to combine the <see cref="IDisposable"/> and <see cref="IScheduler"/> interfaces. This allows Event-Loop schedulers to be disposed. 
    /// </summary>
    public interface IEventLoopScheduler : IScheduler, IDisposable
    {

    }
}