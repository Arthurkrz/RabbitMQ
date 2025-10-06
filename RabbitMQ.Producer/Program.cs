using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var factory = new ConnectionFactory() 
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    ConsumerDispatchConcurrency = 0
};

var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync
(
    queue: "TestQ",
    durable: true,
    exclusive: false,
    autoDelete: false
);

string message = "TestMessage";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync
(
    exchange: "",
    routingKey: "TestQ",
    body: body,
    mandatory: false
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();