﻿namespace LoanProcessing.Infrastructure.RabbitMq;

public class RabbitMqSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string LoanQueue { get; set; } = string.Empty;
}


 