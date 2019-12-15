using System;
using System.Collections.Generic;
using Moni.VelocidadeTempoLocalizacao.Infrastructure.Cache;
using MoniLogs.Core;
using MoniLogs.Core.Commands.Infrastructure.Cache;
using MoniLogs.Core.Commands.Infrastructure.Parameters;
using MoniLogs.Core.Entities;
using MoniLogs.Core.Infrastructure;
using MoniLogs.Core.Services;
using NetMQ;
using NetMQ.Sockets;
using StackExchange.Redis;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class ValidatedMessage
    {
        public ValidatedMessage(Envelope envelope)
        {
            Message = envelope;
            Count = 0;
        }
        
        public int Count { get; set; }
        public Envelope Message { get; set; }
    }
    
    public class Finisher : IFinisherService
    {
        private readonly Dictionary<string, ValidatedMessage> _validatedMessages;
        private readonly ISetCachedValue _setCachedValueCommand;
        private readonly string  PUBLISHER;
        private readonly string ROUTER;
        
        
        public Finisher(string ipRouter, string ipPublisher)
        {
            PUBLISHER = $"tcp://{ipPublisher}:15000";
            ROUTER = $"tcp://{ipRouter}:14000";
            _validatedMessages = new Dictionary<string, ValidatedMessage>();
            _setCachedValueCommand = new RedisSetCachedValue(CreateRedisInstance());
        }

        private IDatabase CreateRedisInstance()
        {
            var redisIp = Environment.GetEnvironmentVariable("CLIENT_REDIS");
            var configString = $"{redisIp}:6379,connectRetry=5";
            var redis = ConnectionMultiplexer.Connect(configString);
            return redis.GetDatabase();
        }
        
        public void Execute()
        {
            using (var reporterFinisher = new PublisherSocket(PUBLISHER))
            using (var jobFinisher = new RouterSocket(ROUTER)) 
            {
                Console.WriteLine("Finisher ONNNNNNNNNNNNNNNNNN");
                Console.WriteLine($"Listening running on: {ROUTER}");
                
                while (true)
                {
                    try
                    {
                        var identity = jobFinisher.ReceiveFrameString();
                        Console.WriteLine("identity: " + identity);
                        var message = jobFinisher.ReceiveFrameString();
                        Console.WriteLine("message: "+ message);
                        
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(message);

                        if (_validatedMessages.ContainsKey(envelope.Identity))
                        {
                            var validatedMessage = _validatedMessages[envelope.Identity];
                            Console.WriteLine("Localizado " + envelope.Identity);
                            
                            validatedMessage.Count++;
                            validatedMessage.Message.ValidationMessages.AddRange(envelope.ValidationMessages);

                            if (validatedMessage.Count == 3)
                            {
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(validatedMessage.Message);
                                reporterFinisher.SendMoreFrame(envelope.Identity).SendFrame(json);
                                Console.WriteLine("Finisher sending " + envelope.Identity);

                                _setCachedValueCommand.Execute(new SetCacheParameter(envelope.Identity, json));
                                
                                _validatedMessages.Remove(envelope.Identity);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Criado " + envelope.Identity);
                            var validated = new ValidatedMessage(envelope);
                            validated.Count++;
                            _validatedMessages.Add(envelope.Identity, validated);
                        }
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