﻿using System;
using Moni.VelocidadeTempoLocalizacao;
using MoniLogs.Core;
using NetMQ;
using NetMQ.Sockets;

namespace validation.empresa
{
    class Program
    {
        static void Main(string[] args)
        {
            var dealer = Environment.GetEnvironmentVariable("VALIDATOR_DEALER");
            var subscriber = Environment.GetEnvironmentVariable("VALIDATOR_SUBSCRIBER");
            
            var validador = new ValidationEmpresa(dealer, subscriber);
            validador.Execute();
        }
    }
}
