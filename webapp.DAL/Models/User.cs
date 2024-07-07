using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapp.DAL.Models
{
    public class User : BaseModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

    }
}
