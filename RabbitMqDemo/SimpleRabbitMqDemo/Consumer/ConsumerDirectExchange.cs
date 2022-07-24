using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Consumer
{
    public class ConsumerDirectExchange
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
                    channel.ExchangeDeclare(exchange: "DirectExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "DirectAllQueue", durable: true, autoDelete: false, arguments: null);

                    //绑定exchange和queue
                    string[] logtypes = new string[] { "debug", "info", "warn", "error" };
                    foreach (string logtype in logtypes)
                    {
                        channel.QueueBind(queue: "DirectAllQueue",
                                exchange: "DirectExChange",
                                routingKey: logtype);
                    }

                    var consumer = new EventingBasicConsumer(channel);

                    //定义消费者
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"接收成功！【{message}】");
                    };

                    //处理消息
                    channel.BasicConsume(queue: "DirectAllQueue", autoAck: true, consumer: consumer);

                }
            }
        }
    }
}
