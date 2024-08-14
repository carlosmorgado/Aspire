using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Microsoft.Extensions.Hosting;

public static class AspireMongoDBDriverExtensions
{
    public static void AddMongoDbClient(
        this IHostApplicationBuilder builder,
        string connectionName)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionName, nameof(connectionName));

        if (builder.Environment.IsDevelopment())
        {
            builder.AddMongoDBClient(connectionName);
            return;
        }

        if (builder.Configuration.GetConnectionString(connectionName) is not string connectionString)
        {
            throw new InvalidOperationException("No connection string found for non Development run.");
        }

        var clientSettings = MongoClientSettings.FromConnectionString(connectionString);

        builder.Services.AddSingleton<IMongoClient>(new MongoClient(clientSettings));
    }
}
