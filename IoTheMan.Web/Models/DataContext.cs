using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IoTheMan.Web.Models
{
    public interface IDataContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<Person> People { get; }
        IMongoCollection<Recipient> Recipients { get; }
        IMongoCollection<Payment> Payments { get; }
        IMongoCollection<DueDateRule> DueDateRules { get; }
    }

    public class DataContext : IDataContext
    {
        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<Person> People
        {
            get { return Database.GetCollection<Person>("People"); }
        }

        public IMongoCollection<Recipient> Recipients
        {
            get { return Database.GetCollection<Recipient>("Recipients"); }
        }

        public IMongoCollection<Payment> Payments
        {
            get { return Database.GetCollection<Payment>("Payments"); }
        }

        public IMongoCollection<DueDateRule> DueDateRules
        {
            get { return Database.GetCollection<DueDateRule>("DueDateRules"); }
        }

        public DataContext()
        {
            var client = new MongoClient("mongodb://localhost");
            Database = client.GetDatabase("MongoMinder");
        }

        public static string NewObjectId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }

    public static class MongoHelperExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IMongoCollection<T> collection)
        {
            var filter = new BsonDocument();

            return await collection.Find(filter).ToListAsync();
        }

        public static async Task<T> FindByIdAsync<T>(this IMongoCollection<T> collection, ObjectId id)
        {
            var fieldDefinition = new StringFieldDefinition<T, object>("_id");
            var filter = Builders<T>.Filter.Eq(fieldDefinition, id);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public static ObjectId ToObjectId(this string id)
        {
            return ObjectId.Parse(id);
        }
    }
}