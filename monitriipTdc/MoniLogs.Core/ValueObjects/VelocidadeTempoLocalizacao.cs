namespace MoniLogs.Core.ValueObjects
{
    public class VelocidadeTempoLocalizacao
    {
        public int idLog { get; set; }
        public string cnpjEmpresaTransporte { get; set; }
        public string placaVeiculo { get; set; }
        public int velocidadeAtual { get; set; }
        public int distanciaPercorrida { get; set; }
        public int situacaoIgnicaoMotor { get; set; }
        public int situacaoPortaVeiculo { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public decimal pdop { get; set; }
        public string dataHoraEvento { get; set; }
        public string smalldatetime { get; set; }
    }
}