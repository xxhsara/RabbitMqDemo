using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Consumer
{
    public class TopicExchangeChina
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
                    channel.ExchangeDeclare(exchange: "TopicExchange", type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "ChinaQueue", durable: true, autoDelete: false, arguments: null);


                    channel.QueueBind(queue: "ChinaQueue", exchange: "TopicExchange", routingKey: "China.#", arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    //定义消费者
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"接收成功！【{message}】");
                    };

                    //处理消息
                    channel.BasicConsume(queue: "ChinaQueue", autoAck: true, consumer: consumer);
                    Console.WriteLine("对来自于中国的消息比较感兴趣的 消费者");

                }
            }
        }
    }
}
