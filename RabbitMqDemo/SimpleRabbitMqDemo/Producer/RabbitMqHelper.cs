using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Producer
{
    public class RabbitMqHelper
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
                    channel.QueueDeclare();//声明队列
                                           //channel.ExchangeDeclare(); //声明交换机类型
                                           //channel.QueueBind();//绑定队列到交换机上

                    //channel.BasicPublish();//发送消息
                }
            }
        }
    }
}
