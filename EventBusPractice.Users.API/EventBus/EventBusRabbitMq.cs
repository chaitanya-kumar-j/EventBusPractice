using RabbitMQ.Client;
using System.Text;

namespace EventBusPractice.Users.API.EventBus
{
    public class EventBusRabbitMq : IEventBus
    {
        public void Publish(string integrationEvent, string eventData)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "users.fanout",
                                             routingKey: integrationEvent,
                                             basicProperties: null,
                                             body: body);
        }
    }
}
