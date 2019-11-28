using System;
using Moni.VelocidadeTempoLocalizacao;

namespace finisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = Environment.GetEnvironmentVariable("JOB_FINISHER_PUBLISHER");
            var router = Environment.GetEnvironmentVariable("JOB_FINISHER_ROUTER");
            
            var jobServer = new Finisher(router, publisher);
            jobServer.Execute();
        }
    }
}
