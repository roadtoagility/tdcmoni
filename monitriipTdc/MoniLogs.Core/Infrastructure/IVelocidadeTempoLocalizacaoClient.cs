using System.Threading.Tasks;
using MoniLogs.Core.Entities;

namespace MoniLogs.Core.Infrastructure
{
    public interface IVelocidadeTempoLocalizacaoClient
    {
        Task<string> SendAsync(Envelope envelope);
        string Send(Envelope envelope);
    }
}