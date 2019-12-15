using System;
using Moni.VelocidadeTempoLocalizacao;
using Microsoft.Extensions.DependencyInjection;
using MoniLogs.Core.Services;

namespace finisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(IFinisherService), new Finisher(Environment.GetEnvironmentVariable("JOB_FINISHER_ROUTER"), Environment.GetEnvironmentVariable("JOB_FINISHER_PUBLISHER")))
                .BuildServiceProvider();

            var jobServer = serviceProvider.GetService<IFinisherService>();
            jobServer.Execute();
        }
    }
}
