using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaSubscriber.Service
{
    public class SubscriberService : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private ConsumerBuilder<string, string> _consumerBuilder;
        public SubscriberService(ConsumerConfig consumerConfig)
        {
            _consumerConfig = consumerConfig;
            _consumerBuilder = new ConsumerBuilder<string, string>(_consumerConfig);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var consumer = _consumerBuilder.Build();
            consumer.Subscribe("LogTopic");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var data = consumer.Consume(cancellationTokenSource.Token);

                    if (data.Message != null)
                    {
                        //Message message = JsonConvert.DeserializeObject<Message>(data.Value);
                        System.Console.WriteLine(data.Value);
                    }
                    await Task.Delay(1000, cancellationToken);
                    //Write logic to process message
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

            }
        }
    }

}
