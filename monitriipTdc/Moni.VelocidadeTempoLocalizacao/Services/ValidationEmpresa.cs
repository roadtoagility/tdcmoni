using System;
using System.Collections.Generic;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using NetMQ;
using NetMQ.Sockets;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class ValidationEmpresa  : IService
    {
        private IDatabase _db;
        private readonly string DELAER;
        private readonly string SUBSCRIBER;
        
        public ValidationEmpresa(string ipDealer, string ipSubscriber)
        {
            DELAER = $">tcp://{ipDealer}:14000";
            SUBSCRIBER = $">tcp://{ipSubscriber}:10000";
            
            var configString = $"172.28.1.6:6379,connectRetry=5";
            var redis = ConnectionMultiplexer.Connect(configString);
            _db = redis.GetDatabase();
        }
        
        public void Execute()
        {
            using (var velocidadeValidationBackend = new DealerSocket(DELAER))
            using (var velocidadeValidationSubscriptionSocket = new SubscriberSocket(SUBSCRIBER))
            {
                velocidadeValidationSubscriptionSocket.Subscribe("validation");
                CreateState();
                Console.WriteLine($"Empresa ONNNNNNNNNNNNNNNNNN");
                Console.WriteLine($"Router running on: {DELAER}");
                Console.WriteLine($"Listening to: {SUBSCRIBER}");

                while (true)
                {
                    try
                    {
                        var topic_velocidade = velocidadeValidationSubscriptionSocket.ReceiveFrameString();
                        var request_to_validate_empresa = velocidadeValidationSubscriptionSocket.ReceiveFrameString();
                        
                        Console.WriteLine($"Log empresa para validacao recebido: {request_to_validate_empresa}");
                        
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(request_to_validate_empresa);
                        envelope.ValidationMessages = GetValidationMessages(envelope.Message);
                        
                        request_to_validate_empresa = Newtonsoft.Json.JsonConvert.SerializeObject(envelope);
                        
                        Console.WriteLine("Sending Empresa: " + request_to_validate_empresa);
                        
                        velocidadeValidationBackend.SendFrame(request_to_validate_empresa);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        
        private List<string> GetValidationMessages(MoniLogs.Core.ValueObjects.VelocidadeTempoLocalizacao log)
        {
            //Todo: adicionar mais validações, mas, a intenção aqui é validar tudo referente a empresa.
            var messages = new List<string>();
            
            try
            {
                var emp = _db.StringGet(log.cnpjEmpresaTransporte);

                if (string.IsNullOrEmpty(emp))
                {
                    messages.Add("Empresa não existe");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return messages;
        }
        
        private void CreateState()
        {
            try
            {
                try
                {
                    _db.StringSet("03658904000105", "03658904000105");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro no rocks: " + ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no rocks: " + ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}