using System;
using System.Collections;
using System.Collections.Generic;
using Moni.VelocidadeTempoLocalizacao.Validations;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using MoniLogs.Core.ValueObjects;
using NetMQ;
using NetMQ.Sockets;

namespace Moni.VelocidadeTempoLocalizacao
{
    public class Validation  : IService
    {
        private readonly string DELAER;
        private readonly string VALIDATION_FRONTEND_ENDPOINT;

        public Validation(string ipDealer, string ipSubscriber)
        {
            DELAER = $">tcp://{ipDealer}:14000";
            VALIDATION_FRONTEND_ENDPOINT = $">tcp://{ipSubscriber}:10000";
        }
        
        
        public void Execute()
        {
            using (var validationBackendSocket = new DealerSocket(DELAER))
            using (var validationfrontEnd = new SubscriberSocket(VALIDATION_FRONTEND_ENDPOINT))
            {
                validationfrontEnd.Subscribe("validation");
                
                Console.WriteLine($"Validation ONNNNNNNNNNNNNNNNNN");
                Console.WriteLine($"Dealer on: {DELAER}");
                Console.WriteLine($"Listening to: {VALIDATION_FRONTEND_ENDPOINT}");
                
                while (true)
                {
                    try
                    {
                        var topic = validationfrontEnd.ReceiveFrameString();
                        var message_to_validate = validationfrontEnd.ReceiveFrameString();
                        
                        
                        var envelope = Newtonsoft.Json.JsonConvert.DeserializeObject<Envelope>(message_to_validate);
                        Console.WriteLine($"Log validacao received: {envelope.Identity}");
                        
                        envelope.ValidationMessages = GetValidationMessages(envelope.Message);
                        message_to_validate = Newtonsoft.Json.JsonConvert.SerializeObject(envelope);

                        Console.WriteLine("Sending Validation: " + envelope.Identity);
                        
                        validationBackendSocket.SendFrame(message_to_validate);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private List<string> GetValidationMessages(MoniLogs.Core.ValueObjects.VelocidadeTempoLocalizacao entidade)
        {
            var erros = new List<string>();
            
            if (CNPJ.IsValid(entidade.cnpjEmpresaTransporte))
            {
                erros.Add("Cnpj inválido");
            }
            
            if (DecimalExato.IsValid(entidade.longitude, 23, 20))
            {
                erros.Add("Longitude inválida");
            }
            
            if (DecimalExato.IsValid(entidade.latitude, 23, 20))
            {
                erros.Add("Latitude inválida");
            }
            
            if (DecimalExato.IsValid(entidade.pdop, 10, 6))
            {
                erros.Add("Latitude inválida");
            }
            
            if (InteiroValido.IsValid(entidade.situacaoIgnicaoMotor))
            {
                erros.Add("Situação Ignção Motor inválido");
            }
            
            if (InteiroValido.IsValid(entidade.situacaoPortaVeiculo))
            {
                erros.Add("Situação Porta Veículo");
            }
            
            if (InteiroValido.IsValid(entidade.distanciaPercorrida))
            {
                erros.Add("Situação Distância Percorrida");
            }
            
            if (Placa.IsValid(entidade.placaVeiculo))
            {
                erros.Add("Placa inválido");
            }
            
            return erros;
        }
    }
}