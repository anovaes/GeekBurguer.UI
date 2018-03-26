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

namespace GeekBurguer.UI.Service
{
    public class Queue
    {
        private static IConfiguration _configuration;

        const string QueueName = "GeekBurger";
        static IQueueClient queueClient;

        public void EnviarMensagem(string msg)
        {
            SendMessagesAsync(msg);
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
                queueClient = new QueueClient(config.ConnectionString, QueueName, ReceiveMode.PeekLock);
                var message = new Message(Encoding.UTF8.GetBytes(mensagem));

                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }


    }
}
