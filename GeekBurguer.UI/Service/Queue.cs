using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurguer.Products.Service;
using GeekBurguer.UI.Service;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using GeekBurguer.UI.Contract;

namespace GeekBurguer.UI.Service
{
    public class Queue
    {
        private static IConfiguration _configuration;

        const string QueueName = "GeekBurger";
        static IQueueClient queueClient;

        public async Task EnviarMensagem(string msg)
        {
            await SendMessagesAsync(msg);
        }

        static async Task SendMessagesAsync(string mensagem)
        {
            try
            {
                _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

                var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
                queueClient = new QueueClient(config.ConnectionString, QueueName);
                var message = new Message(Encoding.UTF8.GetBytes(mensagem));

                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
        //Receber a mensagem na fila
        public static async Task ReceiveAsync()
        {

            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var config = _configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            queueClient = new QueueClient(config.ConnectionString, QueueName, ReceiveMode.PeekLock);
            var handlerOptions = new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false, MaxConcurrentCalls = 3 };
            queueClient.RegisterMessageHandler(MessageHandler, handlerOptions);

            Console.ReadLine();
            Console.WriteLine($"Request to close async");
         
            await queueClient.CloseAsync();
            Console.ReadLine();
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
            return Task.CompletedTask;
        }

        private static async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            if (queueClient.IsClosedOrClosing)
                return;

            var productChangesString = Encoding.UTF8.GetString(message.Body);
            var productChanges = JsonConvert.DeserializeObject<Product>(productChangesString);

            //here message is actually processed
            Thread.Sleep(1500);

            Console.WriteLine($"Message Processed: {productChangesString}");
          
            Task PendingCompleteTask;  
        }
    }
}

