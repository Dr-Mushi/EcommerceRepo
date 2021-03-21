using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestingEFRelations.Models;

namespace TestingEFRelations.Controllers.Consume
{
    public class ProductConsumeController
    {

        public async Task<Product> Index(Product prod)
        {
            var url = "https://localhost:44324/api/ProductApi";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result =  await response.Content.ReadFromJsonAsync<Product>();

            //return result;
            return result;

        }
    }
}
