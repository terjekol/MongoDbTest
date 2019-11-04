using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDbTest.Models
{
    public class ExplorerDbViewModel
    {
        public string[] DatabaseNames { get; set; }
        public string SelectedDatabase { get; set; }
        public string[] CollectionNames { get; set; }
        public string SelectedCollection{ get; set; }
        public List<BsonDocument> Documents { get; set; }
    }
}
