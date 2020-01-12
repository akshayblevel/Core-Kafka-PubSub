using System;
using System.IO;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaSubscriber.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaSubscriber
{
    class Program
    {
        public IConfiguration Configuration { get; }
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath($"{Environment.CurrentDirectory}/../../../"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var consumerConfig = new ConsumerConfig();
            config.Bind("consumer", consumerConfig);


            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, con) =>
            {
                con.AddConfiguration(config);
                if (args != null)
                {
                    con.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();

                services.AddSingleton(consumerConfig);

                services.AddSingleton<IHostedService, SubscriberService>();

            });

            await builder.RunConsoleAsync();
        }
    }
}
