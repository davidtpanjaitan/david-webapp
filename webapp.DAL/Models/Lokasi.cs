using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapp.DAL.Models
{
    public class Lokasi : BaseModel
    {
        public string namaLokasi { get; set; }
        public string namaPetani { get; set; }
        public string lokasiLengkap { get; set; }
        public string koordinat { get; set; }
    }
}
