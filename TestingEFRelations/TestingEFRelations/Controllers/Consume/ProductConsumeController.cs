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
    public class ProductConsumeController : Controller
    {
        IHttpClientFactory _clientFactory;
        IEnumerable<Product> product;
        string errorMessage;

        public ProductConsumeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ActionResult<IEnumerable<Product>>> Index()
        {
            //this is another way to get the api
            HttpClient client = _clientFactory.CreateClient();

            try
            {

                product = await client.GetFromJsonAsync<IEnumerable<Product>>("https://localhost:44324/api/ProductApis");
                errorMessage = null;
            }
            catch(Exception ex)
            {
                errorMessage = $"There was an error getting products : ({ex.Message})";
            }
            @ViewData["errorMessage"] = errorMessage;
            return View(product);

            //This is one way to do it.
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
            //    "https://localhost:44324/api/ProductApis");

            //HttpClient client = _clientFactory.CreateClient();

            //HttpResponseMessage response = await client.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //     product = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            //    errorMessage = null;
            //}
            //else
            //{
            //    //product = new List<Product>();
            //    errorMessage = "there was an error getting products.";
            //}

            //ViewData["errorMessage"] = errorMessage;
            //return View(product);
        }



    }
}




















//var url = "https://localhost:44324/api/ProductApi";
//var httpClient = new HttpClient();
//var response = await httpClient.GetAsync(url);
//var result =  await response.Content.ReadAsStringAsync();

//return result;


//var url = "https://localhost:44324/api/ProductApi";
//var httpClient = new HttpClient();
//var response = await httpClient.GetAsync(url);
//var result =  await response.Content.ReadAsStringAsync();

//return result;

//}


//client.DefaultRequestHeaders.Accept.Add(
//    new MediaTypeWithQualityHeaderValue("application/jsons"));