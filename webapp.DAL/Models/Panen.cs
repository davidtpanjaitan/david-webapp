using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapp.DAL.Models
{
    public class Panen : BaseModel
    {
        public string idLokasi { get; set; }
        public string namaLokasi { get; set; }
        public string jenisMadu { get; set; }
        public string beratPanen { get; set; }
        public string tanggalPanen { get; set; }
        public string beratWarehouse { get; set; }
        public string tanggalWarehouse { get; set; }
        public string catatanWarehouse { get; set; }
        public string status { get; set; }
        public string idPetugasPanen { get; set; }
        public string namaPetugasPanen { get; set; }
        public string idPICPanen { get; set; }
        public string namaPICPanen { get; set; }
        public string idPetugasWarehouse { get; set; }
        public string namaPetugasWarehouse { get; set; }
        public string idAdmin { get; set; }
        public string namaAdmin { get; set; }
    }
}
