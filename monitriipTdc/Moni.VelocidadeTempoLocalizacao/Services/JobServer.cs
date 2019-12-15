using System;
using System.Text;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using MoniLogs.Core.Services;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class JobServer  : IJobServerService
    {
        private readonly string JOB_SERVER_FRONTEND_ENDPOINT;
        private readonly string PUBLISHER;
        
        public JobServer(string ipPublisher, string ipSubscriber)
        {
            JOB_SERVER_FRONTEND_ENDPOINT = $">tcp://{ipSubscriber}:5555";
            PUBLISHER = $"tcp://{ipPublisher}:10000";
        }
        
        public void Execute()
        {
            
            
            using (var jobServerValidationBackend = new PublisherSocket(PUBLISHER)) 
            using (var jobServerFrontend = new SubscriberSocket(JOB_SERVER_FRONTEND_ENDPOINT)) 
            {
                jobServerFrontend.Subscribe("init");
                
                Console.WriteLine("Job server ONNNNNNNNNNNNNNNNNN");
                Console.WriteLine($"Publisher running on: {PUBLISHER}");
                Console.WriteLine($"Listening to: {JOB_SERVER_FRONTEND_ENDPOINT}");
                
                while (true)
                {
                    try
                    {
                        var topic = jobServerFrontend.ReceiveFrameString();
                        var event_job_message = jobServerFrontend.ReceiveFrameString();
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(event_job_message);

                        Console.WriteLine($"Receive: {envelope.Identity}");

                        jobServerValidationBackend.SendMoreFrame("validation").SendFrame(event_job_message);
                    }
                    catch(Exception ex) //Drop message
                    {
                        Console.WriteLine("Erro na validação do dado: " + ex.Message);
                    }
                }
            }
        }
    }
}