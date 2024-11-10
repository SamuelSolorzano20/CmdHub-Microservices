using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration config)
    {
        _config = config;

        var factorty = new ConnectionFactory
        {
            HostName = _config["RabbitMQHost"]!,
            Port = int.Parse(_config["RabbitMQPort"]!),
        };

        try
        {
            _connection = factorty.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "tigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;

            Console.WriteLine("--> Conected to Message Bus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to Message Bus: {ex.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto dto)
    {
        var message = JsonSerializer.Serialize(dto);
        
        if (_connection.IsOpen) 
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending messages...");
            SendMessage(message);
        }
        else 
        {
            Console.WriteLine("--> RabbitM! Connection is Closed, not sending");
        }
    }

    private void SendMessage(string message) 
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body);

        Console.WriteLine($"We have sent {message}");
    }

    public void Dispose() 
    {
        Console.WriteLine("Message Bus Disposed");

        if (_channel.IsOpen) 
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabitMQ Connection Shutdown");
    }

}
