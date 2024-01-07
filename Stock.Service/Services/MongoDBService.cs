using MongoDB.Driver;

namespace Stock.Service.Services
{
    public class MongoDBService
    {
        readonly IMongoDatabase _database;
        readonly IConfiguration _configuration;
        public MongoDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient client = new(_configuration["MongoDB:Server"]);
            _database = client.GetDatabase(_configuration["MongoDB:DatabaseName"]);
        }
        public IMongoCollection<T> GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}
