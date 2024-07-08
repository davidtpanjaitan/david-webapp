using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace david_function.LokasiCont
{
    public class LokasiController
    {
        private readonly IBaseRepository<Lokasi> lokasiRepo;
        private const string routePrefix = "lokasi";

        public LokasiController(IBaseRepository<Lokasi> lokasiRepo)
        {
            this.lokasiRepo = lokasiRepo;
        }

        [FunctionName("GetAllLokasi")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Function, "get", Route = routePrefix)] HttpRequest req)
        {
            var listObject = await lokasiRepo.GetAsync(int.MaxValue, 0);
            return new OkObjectResult(listObject);
        }

        [FunctionName("CreateLokasi")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = routePrefix)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Lokasi lokasi = JsonConvert.DeserializeObject<Lokasi>(requestBody);
            lokasi = await lokasiRepo.CreateAsync(lokasi);
            return new OkObjectResult(lokasi);
        }

        [FunctionName("GetLokasiById")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = routePrefix+"/{id}")] HttpRequest req,
        string id,
        ILogger log)
        {
            Lokasi lokasi = await lokasiRepo.GetByIdAsync(id);
            if (lokasi == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(lokasi);
        }

        [FunctionName("UpdateLokasi")]
        public async Task<IActionResult> UpdateLokasi(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = routePrefix+"/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Lokasi updatedLokasi = JsonConvert.DeserializeObject<Lokasi>(requestBody);
            updatedLokasi.id = id;
            await lokasiRepo.UpdateAsync(updatedLokasi);
            return new OkObjectResult(updatedLokasi);
        }

        [FunctionName("DeleteLokasi")]
        public async Task<IActionResult> DeleteLokasi(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = routePrefix + "/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            await lokasiRepo.DeleteAsync(id);
            return new OkResult();
        }
    }
}
