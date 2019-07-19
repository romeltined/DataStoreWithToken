using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreWithToken.Models
{
    public class DataStoreItem
    {
        public int Id { get; set; }
        public string StoreItemName { get; set; }
        public bool Completed { get; set; }
        public DateTime Created { get; set; }

        public int DataStoreId { get; set; }
        public DataStore DataStore { get; set; }

        public virtual ICollection<ItemDetail> ItemDetails { get; set; }

    }
}
