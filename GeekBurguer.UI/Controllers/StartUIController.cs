using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeekBurguer.UI.Controllers.Configuration;
using GeekBurguer.UI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeekBurguer.UI.Controllers
{
    public class StartUIController : Controller
    {
        StoreCatalogApiConfiguration _storeCatalogConfiguration;
        private Queue messageFila = new Queue();
        bool _storeCatalogOn;
        public StartUIController()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            _storeCatalogConfiguration = config.GetSection("StoreCatalogApi").Get<StoreCatalogApiConfiguration>();
        }

        public async Task<IActionResult> Index()
        {
            while (!_storeCatalogOn)
            {
                VerifyStoreCatalog();
                Thread.Sleep(10000);
            }

            await messageFila.EnviarMensagem("ShowWelcomePage");

            return Ok();
        }

        private async void VerifyStoreCatalog()
        {
            var client = new HttpClient();
            var api = Path.Combine(_storeCatalogConfiguration.StoreApi, _storeCatalogConfiguration.StoreId.ToString());
            HttpResponseMessage response = await client.GetAsync(api);

            _storeCatalogOn = response.StatusCode == HttpStatusCode.OK;
        }
    }
}