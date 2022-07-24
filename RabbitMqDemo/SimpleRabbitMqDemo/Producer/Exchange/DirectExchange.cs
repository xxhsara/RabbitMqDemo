using RabbitMQ.Client;
using SimpleRabbitMqDemo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Exchange
{
    public class DirectExchange
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

                    channel.QueueDeclare(queue: "DirectErrorQueue", durable:true,autoDelete: false, arguments: null);


                    var logtypes = new string[] { "debug", "info", "warn", "error" };

                    foreach(var logtype in logtypes)
                    {
                        channel.QueueBind(queue: "DirectAllQueue", exchange: "DirectExchange", routingKey: logtype);//疑问：是否是根据routingKey一模一样来匹配队列
                    }

                    channel.QueueBind(queue: "DirectErrorQueue", exchange: "DirectExchange", routingKey: "error");

                    var logList = new List<LogMsgDto>();

                    for(int i=1;i<=100;i++)
                    {
                        if (i % 4 == 0)
                        {
                            logList.Add(new LogMsgDto() { LogType = "info", Message = Encoding.UTF8.GetBytes($"info第{i}条信息") });
                        }
                        if (i % 4 == 1)
                        {
                            logList.Add(new LogMsgDto() { LogType = "debug", Message = Encoding.UTF8.GetBytes($"debug第{i}条信息") });
                        }
                        if (i % 4 == 2)
                        {
                            logList.Add(new LogMsgDto() { LogType = "warn", Message = Encoding.UTF8.GetBytes($"warn第{i}条信息") });
                        }
                        if (i % 4 == 3)
                        {
                            logList.Add(new LogMsgDto() { LogType = "error", Message = Encoding.UTF8.GetBytes($"error第{i}条信息") });
                        }
                    }

                    Console.WriteLine("生产者发送100条日志信息");
                    foreach(var log in logList)
                    {
                        channel.BasicPublish(exchange: "DirectExchange", routingKey: log.LogType, basicProperties: null, body: log.Message);
                        Console.WriteLine($"{Encoding.UTF8.GetString(log.Message)}  已发送~~");
                    }
                }
            }
        }
    }
}
