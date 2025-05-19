using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "password",
    VirtualHost = "/"
};

var conn = await factory.CreateConnectionAsync();

using var channel = await conn.CreateChannelAsync();

await channel.QueueDeclareAsync("bookings", durable: true, exclusive: true);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, eventArgs) =>
{
    // byte[]
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"message has been received - {message}");

    await Task.CompletedTask;
};

await channel.BasicConsumeAsync("bookings", true, consumer);
Console.ReadKey();