using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace consoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
//            string onibus_request = @"{
//	                idLog: 1,
//	                cnpjEmpresaTransporte: '03658904000105',
//                    placaVeiculo: 'MVV1523',
//                    velocidadeAtual: 80,
//                    distanciaPercorrida: 100,
//                    situacaoIgnicaoMotor: 1,
//                    situacaoPortaVeiculo: 1,
//                    latitude: -19.513198,
//                    longitude: -47.313823,
//                    pdop: -47.313823,
//                    dataHoraEvento: '20191109',
//                    smalldatetime: '20191109',
//                }";
//
//            var client = new VelocidadeTempLocClient();
//            //            var request = client.Send(onibus_request);
//
//            var request = new List<Task>();
//
//            for (var i = 0; i < 100; i++)
//            {
//                var template = @"{
//	                idLog: " + i + @",
//	                cnpjEmpresaTransporte: '03658904000105',
//                    placaVeiculo: 'MVV1523',
//                    velocidadeAtual: 80,
//                    distanciaPercorrida: 100,
//                    situacaoIgnicaoMotor: 1,
//                    situacaoPortaVeiculo: 1,
//                    latitude: -19.513198,
//                    longitude: -47.313823,
//                    pdop: -47.313823,
//                    dataHoraEvento: '20191109',
//                    smalldatetime: '20191109',
//                }";
//                request.Add(Task.Factory.StartNew(() => client.Send(new Envelope(Guid.NewGuid().ToString(), new VelocidadeTempoLocalizacao()))));
//            }
//
//            Task.WaitAll(request.ToArray());
//
//            Console.WriteLine("FIM");
//            Console.ReadKey();
        }
    }
}
