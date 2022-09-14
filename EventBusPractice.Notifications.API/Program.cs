using EventBusPractice.Notifications.API.Data;
using EventBusPractice.Notifications.API.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ListenForIntegrationEvents(builder);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NotificationsDbContext>(options =>
         options.UseSqlite(builder.Configuration.GetConnectionString("notificationsDbConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();



static void ListenForIntegrationEvents(WebApplicationBuilder builder)
{
    var factory = new ConnectionFactory();
    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var contextOptions = new DbContextOptionsBuilder<NotificationsDbContext>()
            .UseSqlite(builder.Configuration.GetConnectionString("notificationsDbConnection"))
            .Options;
        var dbContext = new NotificationsDbContext(contextOptions);

        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);

        var data = JObject.Parse(message);
        var type = ea.RoutingKey;
        if (type == "user.add")
        {
            dbContext.Users.Add(new User()
            {
                ID = data["id"].Value<int>(),
                Name = data["name"].Value<string>()
            });
            dbContext.SaveChanges();
        }
        else if (type == "user.update")
        {
            var user = dbContext.Users.First(a => a.ID == data["id"].Value<int>());
            user.Name = data["newname"].Value<string>();
            dbContext.SaveChanges();
        }
    };
    channel.BasicConsume(queue: "users.notifications",
                             autoAck: true,
                             consumer: consumer);
}