using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbTest.Models;

namespace MongoDbTest.Services
{
    public class DocumentService
    {
        private readonly MongoClient _client;
        private Dictionary<string, List<string>> _databasesAndCollections;

        public DocumentService(MyDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
        }

        public async Task<Dictionary<string, List<string>>> GetDatabasesAndCollections()
        {
            if (_databasesAndCollections != null) return _databasesAndCollections;
            _databasesAndCollections = new Dictionary<string, List<string>>();
            var databasesResult = _client.ListDatabaseNames();
            await databasesResult.ForEachAsync(async databaseName =>
            {
                var collectionNames = new List<string>();
                var database = _client.GetDatabase(databaseName);
                var collectionNamesResult = database.ListCollectionNames();
                await collectionNamesResult.ForEachAsync(
                    collectionName => { collectionNames.Add(collectionName); });
                _databasesAndCollections.Add(databaseName, collectionNames);
            });
            return _databasesAndCollections;
        }

        /*
        public List<BsonDocument> Get() =>
            _myDocuments.Find(doc => true).ToList();

        public BsonDocument Get(string id) =>
            _myDocuments.Find<BsonDocument>(book => book.Id == id).FirstOrDefault();

        public BsonDocument Create(BsonDocument book)
        {
            _myDocuments.InsertOne(book);
            return book;
        }

        public void Update(string id, BsonDocument bookIn) =>
            _myDocuments.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(BsonDocument bookIn) =>
            _myDocuments.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _myDocuments.DeleteOne(book => book.Id == id);
            */
        public async Task<BsonDocument> GetRow(string databaseName, string collectionName, int index)
        {
            var db = _client.GetDatabase(databaseName);
            var collection = db.GetCollection<BsonDocument>(collectionName);
            BsonDocument document = null;
            await collection.Find(doc => true)
                .Skip(index)
                .Limit(1)
                .ForEachAsync(doc => document = doc);
            return document;
        }

        public async Task<long> GetCollectionCount(string databaseName, string collectionName)
        {
            var db = _client.GetDatabase(databaseName);
            var collection = db.GetCollection<BsonDocument>(collectionName);
            return await collection.EstimatedDocumentCountAsync();
        }
    }
}

