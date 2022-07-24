using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Exchange
{
    public class FanoutExchange
    {
        public void ConnectRabbitMq()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "";
            factory.UserName = "";
            factory.Password = "";
            factory.Port = 0;

            using (IConnection connection = factory.CreateConnection()) //创建链接
            {
                using (IModel channel = connection.CreateModel())//创建信道
                {
                    channel.ExchangeDeclare(exchange: "FanoutExchange", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "FanoutQueue1", durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "FanoutQueue2", durable: true, autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "FanoutQueue1", exchange: "FanoutExchange", routingKey: String.Empty, arguments: null);
                    channel.QueueBind(queue: "FanoutQueue2", exchange: "FanoutExchange", routingKey: String.Empty, arguments: null); //与fanout交换机绑定的queue均会收到消息


                    var messgae = $"当前时间；{DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(messgae);
                    channel.BasicPublish(exchange: "FanoutExchange", routingKey: String.Empty, basicProperties: null, body: body);

                  
                }
            }
        }
    }
}
