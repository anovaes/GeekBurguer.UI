﻿using System;
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
using GeekBurger.Users.Contract;
using Newtonsoft.Json;

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
        private List<FoodRestrictionsList> foodRestrictionsLists;
        public UIController()
        {
            _usuario = new User
            {
                UserId = Guid.NewGuid(),
                Allergies = new List<Allergy>()
                {
                    new Allergy { AllergyId = Guid.NewGuid(), Name = "Carne", Ativo=false},
                    new Allergy { AllergyId = Guid.NewGuid(), Name = "Leite", Ativo=false },
                    new Allergy { AllergyId = Guid.NewGuid(), Name = "Amendoim", Ativo=false }
                }
            };

            _listaEstoque = new List<Store>
            {
                new Store
                {
                    StoreId=Guid.NewGuid(),
                    Name="Chapa",
                    Products=new List<Product>
                    {
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Burguer",
                            Image="burguer.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Pão"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Carne"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Queijo"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Alface"},
                            }
                        },
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Hotdog",
                            Image="hotdog.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Pão"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Salsicha"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Catchup"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Batata Palha"},
                            }
                        }
                    }
                },
                new Store
                {
                    StoreId=Guid.NewGuid(),
                    Name="Fritadeira",
                    Products=new List<Product>
                    {
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Batata Frita",
                            Image="fritas.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Batata"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Óleo"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Sal"},
                            }
                        },
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Nugget",
                            Image="nugget.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Frango"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Farinha"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Óleo"},
                            }
                        }
                    }
                },
                new Store
                {
                    StoreId=Guid.NewGuid(),
                    Name="Sobremesa",
                    Products=new List<Product>
                    {
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Sorvete",
                            Image="sorvete.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Leite"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Chocolate"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Açúcar"},
                            }
                        },
                        new Product
                        {
                            ProductId=Guid.NewGuid(),
                            Name="Torta de maça",
                            Image="torta.jpeg",
                            Items=new List<Item>
                            {
                                new Item{ ItemId=Guid.NewGuid(),Name="Farinha"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Ovo"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Manteiga"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Leite"},
                                new Item{ ItemId=Guid.NewGuid(),Name="Maçã"},
                            }
                        }
                    }
                }
            };
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
            var retorno = foodRestrictionsLists;
            return Ok(foodRestrictionsLists);
        }

        [HttpPost]
        public IActionResult FoodRestriction(Restriction allergy)
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

        //Post envio da lista de produtos por fila
        [HttpPost("{ShowProducts}")]
        public IActionResult ShowProductsList(List<Product> list)
        {
            User user = new User();
            var lista = ApiShowProducts(user);
            string result = JsonConvert.SerializeObject(lista);
            messageFila.EnviarMensagem(result);
            return Ok(lista);
        }

    }
}