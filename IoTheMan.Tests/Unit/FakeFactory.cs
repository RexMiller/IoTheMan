using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using MongoDB.Driver;

namespace IoTheMan.Tests.Unit
{

    public class FakeFactory
    {
        public static IMongoCollection<T> MongoCollection<T>(IList<T> sourceCollection)
        {
            var collection = A.Fake<IMongoCollection<T>>();
            var cursor = A.Fake<IAsyncCursor<T>>();

            A.CallTo(() => cursor.Current)
                .Returns(sourceCollection);

            A.CallTo(() => cursor.MoveNextAsync(A<CancellationToken>._))
                .Returns(Task.FromResult(true));

            A.CallTo(() => collection
                    .FindAsync(A<FilterDefinition<T>>._, A<FindOptions<T, T>>._, A<CancellationToken>._))
                .Returns(cursor);

            A.CallTo(() => collection
                    .FindAsync(A<Expression<Func<T, bool>>>._, A<FindOptions<T, T>>._, A<CancellationToken>._))
                .Returns(cursor);

            return collection;
        }
    }
}
