using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Consumer
{
    public class ConsumerFanoutExchange
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

                    //绑定exchange和queue
                    channel.QueueBind(queue: "FanoutQueue1", exchange: "FanoutExchange", routingKey: string.Empty, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    //定义消费者
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"接收成功！【{message}】");
                    };

                    //处理消息
                    channel.BasicConsume(queue: "FanoutQueue1", autoAck: true, consumer: consumer);

                }
            }
        }
    }
}
