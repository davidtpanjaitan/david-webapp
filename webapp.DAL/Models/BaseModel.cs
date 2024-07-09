using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapp.DAL.Models
{
    public class BaseModel
    {
        public string id { get; set; }
        public DateTime createdDate { get; set; } = DateTime.Now;
        public string createdBy { get; set; }
        public string BasePartition { get; set; } = "";

    }
}
