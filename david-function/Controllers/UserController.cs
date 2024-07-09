using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
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
    public class UserController
    {
        private readonly IBaseRepository<User> userRepo;

        public UserController()
        {
            string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
            string databaseName = Environment.GetEnvironmentVariable("CosmosDbDatabaseName");

            Microsoft.Azure.Cosmos.CosmosClient cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient(cosmosDbConnectionString);
            userRepo = new BaseRepository<User>(cosmosClient, databaseName);
        }

        [FunctionName("GetDaftarUser")]
        public async Task<IActionResult> GetDaftarUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user")] HttpRequest req)
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
            var listObject = await userRepo.GetAsyncPaged(size, num - 1);
            return new OkObjectResult(listObject);
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            user = await userRepo.CreateAsync(user);
            return new OkObjectResult(user);
        }

        [FunctionName("GetUserById")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
        string id,
        ILogger log)
        {
            User user = await userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(user);
        }

        [FunctionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "user/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User updatedUser = JsonConvert.DeserializeObject<User>(requestBody);
            updatedUser.id = id;
            await userRepo.UpdateAsync(updatedUser);
            return new OkObjectResult(updatedUser);
        }

        [FunctionName("DeleteUser")]
        public async Task<IActionResult> DeleteUser(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "user/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            await userRepo.DeleteAsync(id);
            return new OkResult();
        }
    }
}
