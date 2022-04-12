using System;
using Microsoft.Extensions.Logging;

namespace AwsLambdaDI
{
    public class SimpleService : IDisposable
    {
        private readonly string ServiceName;
        private readonly string InstanceId;
        private readonly ILogger<SimpleService> Logger;

        public SimpleService(string name, ILogger<SimpleService> logger)
        {
            ServiceName = name;
            InstanceId = Guid.NewGuid().ToString();
            Logger = logger;

            logger.LogInformation("Created {ServiceName}, {Id}.", ServiceName, InstanceId);
        }

        public void DoSomething()
        {
            Logger.LogInformation("{ServiceName} {Id} is doing something.", ServiceName, InstanceId);
        }

        public void Dispose()
        {
            Logger.LogInformation("Disposing {ServiceName} {Id}.", ServiceName, InstanceId);
        }
    }

    public class SingletonService : SimpleService
    {
        public SingletonService(ILogger<SingletonService> logger) : base("SingletonService", logger) { }
    }

    public class ScopedService : SimpleService
    {
        public ScopedService(ILogger<ScopedService> logger) : base("ScopedService", logger) { }
    }

    public class TransientService : SimpleService
    {
        public TransientService(ILogger<TransientService> logger) : base("TransientService", logger) { }
    }
}
