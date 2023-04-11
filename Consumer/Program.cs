using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "pedidos",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {

        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var pedido = JsonConvert.DeserializeObject<Pedido>(message);

        System.Console.WriteLine($"Pedido Recebido: Id={pedido.Id} Produto: {pedido.Produto}");
        Console.WriteLine("Processando pedido...");
        Console.WriteLine("Pedido processado!");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };
    channel.BasicConsume(queue: "pedidos",
                                     autoAck: false,
                                     consumer: consumer);

    Console.WriteLine("Aguardando pedidos...");
    Console.ReadLine();

}