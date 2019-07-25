using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class TokenResponse
    {
        public string TokenValue { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
    }
}
