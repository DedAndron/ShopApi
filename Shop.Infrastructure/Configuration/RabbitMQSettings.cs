using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Configuration;

public sealed class RabbitMQSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
}
