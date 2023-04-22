using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmail;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly EmailRepository _emailRepo;

        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;


        public AzureServiceBusConsumer(EmailRepository emailRepo, IConfiguration _configuration)
        {
            _emailRepo = emailRepo;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");


            var client = new ServiceBusClient(serviceBusConnectionString);

            orderUpdatePaymentStatusProcessor = client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionEmail);
        }
        public async Task Start()
        {
            orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();

        }
        public async Task Stop()
        {
            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage objMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);
            try
            {
                await _emailRepo.SendAndLogEmail(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        

    }
}
