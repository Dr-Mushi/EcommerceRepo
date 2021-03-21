using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TestingEFRelations.Controllers.Consume
{
    public class ProductConsumeController
    {
        IHttpClientFactory _clientFactory;

        public ProductConsumeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Product>> Index()
        
        {
            //var url = "https://localhost:44324/api/ProductApi";
            //var httpClient = new HttpClient();
            //var response = await httpClient.GetAsync(url);
            //var result =  await response.Content.ReadAsStringAsync();

            //return result;

            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://localhost:44324/api/ProductApi");

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //var responseStream =await response.Content.ReadAsStreamAsync();
            //IEnumerable<Product> product = await JsonSerializer.DeserializeAsync<IEnumerable<Product>>(responseStream);
            //var product = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();

            var product = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            //}
            return product;

        }
    }
}
