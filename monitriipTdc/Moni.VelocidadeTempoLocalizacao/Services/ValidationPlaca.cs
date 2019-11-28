using System;
using System.Collections.Generic;
using System.IO;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using NetMQ;
using NetMQ.Sockets;
using RocksDbSharp;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao 
{
    public class ValidationPlaca  : IService
    {
        
        private IDatabase _db;
        private readonly string DELAER;
        private readonly string SUBSCRIBER;
        
        public ValidationPlaca(string ipDealer, string ipSubscriber)
        {
            DELAER = $">tcp://{ipDealer}:14000";
            SUBSCRIBER = $">tcp://{ipSubscriber}:10000";
            
            var configString = $"172.28.1.6:6379,connectRetry=5";
            var redis = ConnectionMultiplexer.Connect(configString);
            _db = redis.GetDatabase();
        }
        
        public void Execute()
        {
            using (var placaValidationBackend = new DealerSocket(DELAER))
            using (var placaValidationSubcriptionSocket = new SubscriberSocket(SUBSCRIBER))
            {
                placaValidationSubcriptionSocket.Subscribe("validation");
                
                CreateState();
                
                Console.WriteLine($"Placa on");
                Console.WriteLine($"Delaer running on: {DELAER}");
                Console.WriteLine($"Listening to: {SUBSCRIBER}");
                
                while (true)
                {
                    try
                    {
                        var topic_placa = placaValidationSubcriptionSocket.ReceiveFrameString();
                        var request_to_validate_placa = placaValidationSubcriptionSocket.ReceiveFrameString();
                        Console.WriteLine($"Log placa para validacao recebido: {request_to_validate_placa}");
                        
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(request_to_validate_placa);
                        
                        envelope.ValidationMessages = GetValidationMessages(envelope.Message);
                        
                        request_to_validate_placa = Newtonsoft.Json.JsonConvert.SerializeObject(envelope);
                        
                        Console.WriteLine("Sending Placa: " + request_to_validate_placa);
                        
                        placaValidationBackend.SendFrame(request_to_validate_placa);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void CreateState()
        {
            try
            {
                _db.StringSet("MVV1523", "MVV1523");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no rocks: " + ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        private List<string> GetValidationMessages(MoniLogs.Core.ValueObjects.VelocidadeTempoLocalizacao log)
        {
            //Todo: adicionar mais validações, mas, a intenção aqui é validar tudo referente ao veículo, placa etc..
            var messages = new List<string>();
            
            try
            {
                var emp = _db.StringGet(log.placaVeiculo);

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
    }
}