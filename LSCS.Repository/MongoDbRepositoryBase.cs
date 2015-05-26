
using System;
using MongoDB.Driver;

namespace LSCS.Repository
{
    public abstract class MongoDbRepositoryBase
    {
        private static MongoClient _client;

        protected MongoDatabase Database { get; set; }

        protected MongoDbRepositoryBase(string connectionString, string databaseName)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");

            if (_client == null)
            {
                _client = new MongoClient(connectionString);
            }
            Database = _client.GetServer().GetDatabase(databaseName);
        }       
    }
}
