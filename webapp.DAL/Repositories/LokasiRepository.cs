using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapp.DAL.Models;

namespace webapp.DAL.Repositories
{
    public class LokasiRepository : BaseRepository<Lokasi>
    {
        public LokasiRepository(CosmosClient client, string databaseName) : base(client, databaseName)
        {
        }
        public async Task<List<Lokasi>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Lokasi>("SELECT * FROM c ORDER BY c.namaLokasi");
            var response = await query.ReadNextAsync();
            return response.ToList();
        }
    }
}
