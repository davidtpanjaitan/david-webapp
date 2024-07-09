using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
    public class PanenController
    {
        private readonly IBaseRepository<Panen> panenRepo;

        public PanenController() 
        {
            string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
            string databaseName = Environment.GetEnvironmentVariable("CosmosDbDatabaseName");

            CosmosClient cosmosClient = new CosmosClient(cosmosDbConnectionString);
            panenRepo = new BaseRepository<Panen>(cosmosClient, databaseName);
        }

        [FunctionName("GetDaftarPanen")]
        public async Task<IActionResult> GetDaftarPanen([HttpTrigger(AuthorizationLevel.Function, "get", Route = "panen")] HttpRequest req)
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
            var listObject = await panenRepo.GetAsyncPaged(size, num-1);
            return new OkObjectResult(listObject);
        }

        [FunctionName("CreatePanen")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "panen")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Panen panen = JsonConvert.DeserializeObject<Panen>(requestBody);
            panen = await panenRepo.CreateAsync(panen);
            return new OkObjectResult(panen);
        }

        [FunctionName("GetPanenById")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "panen/{id}")] HttpRequest req,
        string id,
        ILogger log)
        {
            Panen panen = await panenRepo.GetByIdAsync(id);
            if (panen == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(panen);
        }

        [FunctionName("UpdatePanen")]
        public async Task<IActionResult> UpdatePanen(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "panen/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Panen updatedPanen = JsonConvert.DeserializeObject<Panen>(requestBody);
            updatedPanen.id = id;
            await panenRepo.UpdateAsync(updatedPanen);
            return new OkObjectResult(updatedPanen);
        }

        [FunctionName("DeletePanen")]
        public async Task<IActionResult> DeletePanen(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "panen/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            await panenRepo.DeleteAsync(id);
            return new OkResult();
        }
    }
}
