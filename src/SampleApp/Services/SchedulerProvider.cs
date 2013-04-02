using System.Reactive.Concurrency;
using Sample.Core;

namespace SampleApp.Services
{
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        private readonly IScheduler _dispatcherScheduler;

        public SchedulerProvider()
        {
            var currentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            _dispatcherScheduler = new DispatcherScheduler(currentDispatcher);
        }

        public IScheduler Dispatcher
        {
            get { return _dispatcherScheduler; }
        }

        public IScheduler Concurrent
        {
            get { return TaskPoolScheduler.Default; }
        }

        public ISchedulerLongRunning LongRunning
        {
            get { return TaskPoolScheduler.Default; }
        }

        public IEventLoopScheduler CreateEventLoopScheduler(string name)
        {
            return new EventLoopSchedulerWrapper(name);
        }
    }
}