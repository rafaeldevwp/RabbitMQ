using RabbitMQ.Client;
using System.IO;
using System.Text;

var factory = new ConnectionFactory{HostName = "localhost"};
using (var conection  = factory.CreateConnection())
using (var channel = conection.CreateModel())
{
    channel.QueueDeclare(queue: "pedidos",
    durable: false,
    exclusive:false,
    autoDelete:false,
    arguments:null);

    var pedido = new Pedido { Id = 1, Produto = "Mouse", Quantidade = 1, Data = DateTime.Now};

    string Mensage = Newtonsoft.Json.JsonConvert.SerializeObject(pedido);
    var body = Encoding.UTF8.GetBytes(Mensage);

   channel.BasicPublish(exchange:"",
   routingKey:"pedidos",
   basicProperties:null,
   body:body);

   System.Console.WriteLine("Pedido enviado: {0}",Mensage);


}