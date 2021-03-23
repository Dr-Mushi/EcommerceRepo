using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;

namespace TestingEFRelations.Controllers.Consume
{
    public class ProductConsumeController : Controller
    {
        IHttpClientFactory _clientFactory;
        private readonly IProductRepository _product;
        IEnumerable<Product> products;
        Product product;
        string errorMessage;

        public ProductConsumeController(IHttpClientFactory clientFactory , IProductRepository product)
        {
            _clientFactory = clientFactory;
            _product = product;
        }

        public async Task<ActionResult<IEnumerable<Product>>> Index()
        {
            //this is another way to get the api
            HttpClient client = _clientFactory.CreateClient("Ecommerce");

            try
            {
                products = await client.GetFromJsonAsync<IEnumerable<Product>>("ProductApi");
                errorMessage = null;
            }
            catch(Exception ex)
            {
                errorMessage = $"There was an error getting products : ({ex.Message})";
            }
            @ViewData["errorMessage"] = errorMessage;
            return View(products);
        }

        
        public async Task<ActionResult<Product>> Details(int id)
        {
            //this is another way to get the api
            HttpClient client = _clientFactory.CreateClient("Ecommerce");

            try
            {
                product = await client.GetFromJsonAsync<Product>($"ProductApi/{id}");
                errorMessage = null;
            }
            catch (Exception ex)
            {
                errorMessage = $"There was an error getting products : ({ex.Message})";
            }
            @ViewData["errorMessage"] = errorMessage;
            return View(product);
           
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["SizeName"] = _product.SelectListSize();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Product product)
        {
           
            //initiate Lists so that it won't give null errors
            product.ImageFileName = new List<string>();
            product.ImageFileExtension = new List<string>();
            product.ImageFileData = new List<string>();
            //for multiple images upload
            for (int i  = 0; i < product.ImageFile.Count; i++)
            {
                product.ImageFileName.Add(product.ImageFile[i].FileName);
                product.ImageFileExtension.Add(Path.GetExtension(product.ImageFile[i].FileName));
                if (product.ImageFile[i].Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await product.ImageFile[i].CopyToAsync(ms);
                        byte[] fileBytes = ms.ToArray();
                        //get the file as a string
                        product.ImageFileData.Add(Convert.ToBase64String(fileBytes));
                    }
                }
            }

            HttpClient client = _clientFactory.CreateClient("Ecommerce");

            var response = await client.PostAsJsonAsync("ProductApi",product);

            if (response.IsSuccessStatusCode)
            {
                errorMessage = "";
            }
            else
            {
                errorMessage = $"There was an error getting products : ({response.StatusCode})";
            }

            return RedirectToAction(nameof(Index)); 
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpClient client = _clientFactory.CreateClient("Ecommerce");
            product = await client.GetFromJsonAsync<Product>($"ProductApi/{id}");
            ViewData["SizeName"] = _product.SelectListSize();
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            HttpClient client = _clientFactory.CreateClient("Ecommerce");

            var patchDoc = new JsonPatchDocument<Product>();
            patchDoc.Replace(p => p.ProductName,product.ProductName);
            patchDoc.Replace(p => p.ProductDescription, product.ProductDescription);
            patchDoc.Replace(p => p.SizeID, product.SizeID);
            patchDoc.Replace(p => p.ProductQuantity, product.ProductQuantity);
            patchDoc.Replace(p => p.ProductPrice, product.ProductPrice);


            //turn the value into JSON
            var serializedDoc = JsonConvert.SerializeObject(patchDoc);
            var productToJson = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
            
            
            var response = await client.PatchAsync($"ProductApi/{id}", productToJson);

            if (response.IsSuccessStatusCode)
            {
                errorMessage = null;
            }
            else
            {
                errorMessage = $"There was an error updating product : ({response.StatusCode})";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}









//This is one way to get API.
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





//////////////////////////////////////////////////////////////////////
////this is another way to post the api
//HttpClient client = _clientFactory.CreateClient("Ecommerce");

//var result = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

//var request = await client.PostAsync("ProductApi", result);

////HttpResponseMessage response = await client.SendAsync(request);


//if (request.IsSuccessStatusCode)
//{
//    errorMessage = null;
//}
//else
//{
//    errorMessage = $"There was an error getting products : ({request.StatusCode})";
//}

//@ViewData["errorMessage"] = errorMessage;
//return View(nameof(Index));

//////////////////////////////////////////////////////////////////////////////
//for one image upload
//foreach (var item in product.ImageFile)
//{
//    product.ImageFileName = item.FileName;
//    product.ImageFileExtension = Path.GetExtension(item.FileName);
//    if(item.Length > 0)
//    {
//        using(var ms = new MemoryStream())
//        {
//           await item.CopyToAsync(ms);
//            byte[] fileBytes = ms.ToArray();
//            product.ImageFileData = Convert.ToBase64String(fileBytes);
//        }
//    }
//}

//HttpClient client = _clientFactory.CreateClient("Ecommerce");
//var response = await client.PostAsJsonAsync("ProductApi", product);

//try
//{
//    var created = await response.Content.ReadFromJsonAsync<Product>();
//    errorMessage = null;
//}
//catch (Exception ex)
//{
//    errorMessage = $"There was an error getting products : ({ex.Message})";
//}
//@ViewData["errorMessage"] = errorMessage;
//return View(product);

///////////////////////////////////////////////////////////////////////////////////

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