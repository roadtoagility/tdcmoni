using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moni.VelocidadeTempoLocalizacao;
using MoniLogs.Core;
using MoniLogs.Core.Infrastructure;
using NetMQ;
using NetMQ.Sockets;

namespace clientApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var publishervar = Environment.GetEnvironmentVariable("CLIENT_API_PUBLISHER");
            var finishervar = Environment.GetEnvironmentVariable("JOB_FINISHER_ROUTER");
            
            var sender = new PublisherSocket($"@tcp://{publishervar}:5555");
            var finisher = new SubscriberSocket($">tcp://{finishervar}:15000");
            
            System.Diagnostics.Debug.WriteLine("Web server listening to:" + $">tcp://{finishervar}:15000");
            IFinisherSubscription finishersub = new FinisherSubscription(finisher);
            
            builder.RegisterInstance(finishersub).SingleInstance();
            builder.RegisterInstance(sender).SingleInstance();
            builder.RegisterType<VelocidadeTempLocClient>().As<IVelocidadeTempoLocalizacaoClient>();
            builder.RegisterType<CacheGateway>().As<ICacheGateway>();
        }
    }
}
