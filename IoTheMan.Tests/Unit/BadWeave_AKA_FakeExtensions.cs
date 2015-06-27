using FakeItEasy;
using MongoDB.Driver;

namespace IoTheMan.Tests.Unit
{
    // ReSharper disable once InconsistentNaming
    internal static class BadWeave_AKA_FakeExtensions
    {
        public static void VerifyInsertOneAsync<T>(this IMongoCollection<T> collection)
        {
            A.CallTo(collection)
                .Where(c => c.Method.Name == "InsertOneAsync")
                .MustHaveHappened();
        }

        public static void VerifyUpdateOneAsync<T>(this IMongoCollection<T> collection)
        {
            A.CallTo(collection)
                .Where(c => c.Method.Name == "UpdateOneAsync")
                .MustHaveHappened();
        }

        public static void VerifyFindOneAndUpdateAsync<T>(this IMongoCollection<T> collection)
        {
            A.CallTo(collection)
                .Where(c => c.Method.Name == "FindOneAndUpdateAsync")
                .MustHaveHappened();
        }

        public static void VerifyFindOneAndReplaceAsync<T>(this IMongoCollection<T> collection)
        {
            A.CallTo(collection)
                .Where(c => c.Method.Name == "FindOneAndReplaceAsync")
                .MustHaveHappened();
        }

        public static void VerifyFindByIdAsync<T>(this IMongoCollection<T> collection)
        {
            A.CallTo(collection)
                .Where(c => c.Method.Name == "FindAsync")
                .MustHaveHappened();
        }
    }
}