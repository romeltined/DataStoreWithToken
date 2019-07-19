using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string OtpHash { get; set; }
        public string TokenHash { get; set; }
        public bool Activated { get; set; }
        public bool Revoked { get; set; }

        public int DataStoreId { get; set; }
        public DataStore DataStore { get; set; }


    }
}
