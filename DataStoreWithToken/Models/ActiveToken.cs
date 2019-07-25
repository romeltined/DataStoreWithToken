using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class ActiveToken
    {
        public int Id { get; set; }
        public string Otp { get; set; }
        public string Source { get; set; }
        public DateTime Created { get; set; }
    }
}
