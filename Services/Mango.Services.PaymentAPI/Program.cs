using Mango.MessageBus;
using Mango.Services.PaymentAPI.Extension;
using Mango.Services.PaymentAPI.Messaging;
using PaymentProcessor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//AZURE SERV�CE
builder.Services.AddSingleton<IProcessPayment,ProcessPayment>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//AZURE SERV�CE
app.UseAzureServiceBusConsumer();

app.Run();
