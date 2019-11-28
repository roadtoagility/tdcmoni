using System.Collections.Generic;
using MoniLogs.Core.ValueObjects;

namespace MoniLogs.Core.Entities
{
    public class Envelope
    {
        public string Identity { get; set; }
        public VelocidadeTempoLocalizacao Message { get; set; }
        public List<string> ValidationMessages { get; set; }

        public Envelope(string identity, VelocidadeTempoLocalizacao message)
            :this()
        {
            Identity = identity;
            Message = message;
        }

        public Envelope()
        {
            ValidationMessages = new List<string>();
        }
    }
}