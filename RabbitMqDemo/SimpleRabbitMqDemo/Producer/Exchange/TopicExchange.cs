using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Exchange
{
    public class TopicExchange
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

                    channel.QueueDeclare(queue:"ChinaQueue",durable: true, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue:"newsQueue",durable:true,autoDelete: false, arguments: null);

                    channel.QueueBind(queue: "ChinaQueue", exchange: "TopicExchange",routingKey:"China.#",arguments:null);//疑问：（1）是否对大小写敏感。 (2)channel.ExchangeBind这个方法是否可以使用


                    channel.QueueBind(queue: "newsQueue", exchange: "TopicExchange", routingKey: "#.news", arguments: null);

                    {
                        string messgae = "来自中国的消息...";
                        var body = Encoding.UTF8.GetBytes(messgae);

                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "China.news", basicProperties: null, body: body);//是否是会发送到routing相匹配的所有的queue上

                        Console.WriteLine($"消息{messgae}已发送到队列");
                    }
                   


                    {
                        string message = "来自中国的天气消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "China.weather", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                    {
                        string message = "来自美国的新闻消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "usa.news", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                    {
                        string message = "来自美国的天气消息。。。。";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "TopicExchange", routingKey: "usa.weather", basicProperties: null, body: body);
                        Console.WriteLine($"消息【{message}】已发送到队列");
                    }
                }
            }
        }
    }
}
