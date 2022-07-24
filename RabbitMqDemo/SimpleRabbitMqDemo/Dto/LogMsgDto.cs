using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRabbitMqDemo.Dto
{
    public class LogMsgDto
    {
        public byte[] Message { get; set; }

        public string LogType { get; set; }
    }
}
