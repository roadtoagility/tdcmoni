using System;
using System.Text;
using Moni.VelocidadeTempoLocalizacao;
using NetMQ;
using NetMQ.Sockets;

namespace jobServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = Environment.GetEnvironmentVariable("JOB_SERVER_PUBLISHER");
            var subscriber = Environment.GetEnvironmentVariable("JOB_SERVER_SUBSCRIBER");
            
            var jobServer = new JobServer(publisher, subscriber);
            jobServer.Execute();
        }
    }
}
