using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapp.DAL.Models;
using webapp.DAL.Repositories;

namespace david_function.Controllers
{
    public class ProdukController
    {
        private readonly IBaseRepository<Produk> produkRepo;

        public ProdukController()
        {
            string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
            string databaseName = Environment.GetEnvironmentVariable("CosmosDbDatabaseName");

            CosmosClient cosmosClient = new CosmosClient(cosmosDbConnectionString);
            produkRepo = new BaseRepository<Produk>(cosmosClient, databaseName);
        }

        [FunctionName("GetDaftarProduk")]
        public async Task<IActionResult> GetDaftarProduk([HttpTrigger(AuthorizationLevel.Function, "get", Route = "produk")] HttpRequest req)
        {
            var queryParam = req.GetQueryParameterDictionary();
            string pageSize;
            if (!queryParam.TryGetValue("pageSize", out pageSize))
            {
                pageSize = "20";
            }
            if (!queryParam.TryGetValue("pageNum", out string pageNum))
            {
                pageNum = "1";
            };
            int.TryParse(pageSize, out int size);
            int.TryParse(pageNum, out int num);
            var listObject = await produkRepo.GetAsyncPaged(size, num - 1);
            return new OkObjectResult(listObject);
        }

        [FunctionName("CreateProduk")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "produk")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Produk produk = JsonConvert.DeserializeObject<Produk>(requestBody);
            produk = await produkRepo.CreateAsync(produk);
            return new OkObjectResult(produk);
        }

        [FunctionName("GetProdukById")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "produk/{id}")] HttpRequest req,
        string id,
        ILogger log)
        {
            Produk produk = await produkRepo.GetByIdAsync(id);
            if (produk == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(produk);
        }

        [FunctionName("UpdateProduk")]
        public async Task<IActionResult> UpdateProduk(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "produk/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Produk updatedProduk = JsonConvert.DeserializeObject<Produk>(requestBody);
            updatedProduk.id = id;
            await produkRepo.UpdateAsync(updatedProduk);
            return new OkObjectResult(updatedProduk);
        }

        [FunctionName("DeleteProduk")]
        public async Task<IActionResult> DeleteProduk(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "produk/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            await produkRepo.DeleteAsync(id);
            return new OkResult();
        }
    }
}
