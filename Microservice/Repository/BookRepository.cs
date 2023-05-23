using Microservice.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Microservice.Repository;

public interface IBooksRepository
{
     Task<List<Book>> GetAsync();

     Task<Book?> GetAsync(string id);

     Task CreateAsync(Book newBook);

     Task UpdateAsync(string id, Book updatedBook);

     Task RemoveAsync(string id);
}

public class BooksRepository : IBooksRepository
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        MongoCredential? credential = MongoCredential.CreateCredential(
            databaseName: databaseSettings.Value.DatabaseName, 
            username: databaseSettings.Value.User, 
            password: databaseSettings.Value.Password);

        MongoClientSettings? settings = new MongoClientSettings
        {
            Credential = credential,
            Server = new MongoServerAddress(
                host: databaseSettings.Value.Host, 
                port: databaseSettings.Value.Port),
        };

        var mongoClient = new MongoClient(settings);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            databaseSettings.Value.CollectionName);
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}