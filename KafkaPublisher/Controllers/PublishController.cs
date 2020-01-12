using Confluent.Kafka;
using KafkaPublisher.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace KafkaPublisher.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private ProducerBuilder<string, string> _producer;
        private ProducerConfig _config;
        public PublishController(ProducerConfig config)
        {
            _config = config;
            _producer = new ProducerBuilder<string, string>(this._config);
        }
        /// <summary>
        /// Publish Message
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PublishAsync(string topic,[FromBody] Message message)
        {
            try
            {
                var serializedMessage = JsonConvert.SerializeObject(message);
                var producer = _producer.Build();
                await producer.ProduceAsync(topic, new Message<string, string>()
                {
                    Key = System.Guid.NewGuid().ToString(),
                    Value = serializedMessage
                });

                return Ok("Your message is published.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}