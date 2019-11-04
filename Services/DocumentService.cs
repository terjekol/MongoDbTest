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
        private MongoClient _client;
        public Dictionary<string, List<string>> _databasesAndCollections;
        private readonly MyDatabaseSettings _settings;

        public DocumentService(MyDatabaseSettings settings)
        {
            _settings = settings;
        }

        public async Task<Dictionary<string, List<string>>> GetDatabasesAndCollections()
        {
            if (_databasesAndCollections != null) return _databasesAndCollections;
            _databasesAndCollections = new Dictionary<string, List<string>>();
            _client = new MongoClient(_settings.ConnectionString);
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
    }
}

