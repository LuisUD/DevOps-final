using Microservice.Models;
using Microsoft.Extensions.Options;

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

    private List<Book> _booksCollection;

    public BooksRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        _booksCollection = new List<Book> {};
    }

    public async Task<List<Book>> GetAsync() => _booksCollection;

    public async Task<Book?> GetAsync(string id) =>
        _booksCollection.Find(x => x.Id == id);

    public async Task CreateAsync(Book newBook) =>
        _booksCollection.Add(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) 
    {
        Book? book = _booksCollection.Find(x => x.Id == id);
        _booksCollection.Remove(book);
        _booksCollection.Add(updatedBook);
    }

    public async Task RemoveAsync(string id) {

         Book? book = _booksCollection.Find(x => x.Id == id);
         _booksCollection.Remove(book);
    }
        
}