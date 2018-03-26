using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GeekBurguer.UI.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using GeekBurguer.UI.Controllers.Configuration;
using GeekBurguer.UI.Service;

namespace GeekBurguer.UI.Controllers
{
    [Produces("application/json")]
    [Route("api/UI")]
    public class UIController : Controller
    {
        private Contract.User _usuario;
        private List<Store> _listaEstoque;
        static HttpClient client = new HttpClient();
        private List<Product> _listaProdutos;
        private Queue messageFila = new Queue();

        public UIController()
        {
           

        }

        [HttpGet("{userFace}")]
        public IActionResult IndentifyUser(byte[] userFace)
        {
            if (ApiIndentifyUser(userFace))
            {
                return Ok(_listaEstoque);
            }
            else
            {
                return Ok(_usuario);
            }
        }

        public IActionResult ChoseRestriction(Contract.User user)
        {
            var lista = ApiChoseRestriction(user);
            return Ok(lista);
        }

        private List<Product> ApiShowProducts(User user)
        {
            return _listaProdutos;
        }

        ////Nova Ordem
        [HttpGet]
        public IActionResult Order(Order order)
        {
            var pedido = ApiPostOrder(order);
            return Ok(pedido);
        }

        [HttpGet]
        public IActionResult ShowFoodRestrictionsForm(User user)
        {
            return Ok();
        }

        public IActionResult FoodRestriction(Allergy user)
        {
            return Ok();
        }

        public IActionResult ApiFoodRestriction(User user)
        {
            return Ok();
        }


        private bool ApiIndentifyUser(byte[] userFace)
        {
            return false;
        }

        private List<Store> ApiChoseRestriction(Contract.User user)
        {
            return _listaEstoque;
        }

        private int ApiPostOrder(Order order)
        {
            return 1354;
        }
        private int ApiPostOrder(List<Order> order)
        {
            return 2000;
        }
        public async Task<IActionResult> GetProductUser(List<Order> order)
        {
            HttpResponseMessage response = await client.GetAsync($"api/products/id");
            return Ok(response);
        }

        [HttpPost("{ShowProducts}")]
        public IActionResult ShowProductsList(List<Product> list)
        {
            User user = new User();
            var lista = ApiShowProducts(user);
            messageFila.EnviarMensagem("ShowProducts");
            return Ok(lista);
        }

    }
}