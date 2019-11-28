using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoniLogs.Core;
using MoniLogs.Core.Entities;
using MoniLogs.Core.Infrastructure;
using MoniLogs.Core.ValueObjects;
using RocksDbSharp;

namespace clientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IVelocidadeTempoLocalizacaoClient _client;
        private readonly ICacheGateway _cache;

        public LogController(IVelocidadeTempoLocalizacaoClient client, ICacheGateway cache)
        {
            _client = client;
            _cache = cache;
        }
        
        [HttpGet]
        [Route("{log}")]
        public async Task<string> Get(string log)
        {
            return await _cache.GetValue(log);
        }

        [HttpPost]
       public async Task<IActionResult> Post(VelocidadeTempoLocalizacao log)
        {
            var result = await _client.SendAsync(new Envelope(Guid.NewGuid().ToString(), log));
            return Created("/", result);
        }
    }
}