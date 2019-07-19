using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class DataStore
    {
        public int Id { get; set; }
        public string StoreName { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<DataStoreItem> DataStoreItems {get;set;}
        
    }
}
