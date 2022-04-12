using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsLambdaDI
{
    public class Function
    {
        private ServiceProvider _serviceProvider;

        public Function()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void FunctionHandler(ILambdaContext context)
        {
            using var scope = GetServiceScope();

            for (int i = 0; i < 5; i++)
            {
                var singleton = scope.ServiceProvider.GetService<SingletonService>();
                singleton.DoSomething();

                var scoped = scope.ServiceProvider.GetService<ScopedService>();
                scoped.DoSomething();

                var transient = scope.ServiceProvider.GetService<TransientService>();
                transient.DoSomething();
            }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddLambdaLogger();
            });
            serviceCollection.AddSingleton<SingletonService>();
            serviceCollection.AddScoped<ScopedService>();
            serviceCollection.AddTransient<TransientService>();
        }

        public IServiceScope GetServiceScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}