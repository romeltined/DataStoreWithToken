using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class ItemDetail
    {
        public int Id { get; set; }
        public string ItemDetailName { get; set; }
        public string ItemDetailValue { get; set; }

        public bool Cancelled { get; set; }
        public DateTime Created { get; set; }


        public int DataStoreItemId { get; set; }

        [JsonIgnore]
        public DataStoreItem DataStoreItem { get; set; }
    }
}
