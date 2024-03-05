using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LibraryApp.DataAccess.Models;

namespace LibraryApp.DataAccess.Repositories
{
    public interface IBookRepository
    {
        Task<int> AddBook(Book book);
        Task ChangeBookData(Book book);
        Task<Book> GetBookOrDefault(int bookId);
        Task<IEnumerable<Book>> GetBooks(int page = 0);
        Task<ClientBook> GetClientBookOrDefault(int bookId, int clientId);
    }

    public class BookRepository : IBookRepository
    {
        readonly IConnectionFactory _connectionFactory;

        IDbConnection Connection => _connectionFactory.GetDbConnection();
        
        public BookRepository(IConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<int> AddBook(Book book)
        {
            
            
            var res = await Connection.QuerySingleAsync<int>(
                @"
                    INSERT INTO Book (Name, Description) 
                    VALUES (@Name, @Description);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ", book);
            return res;
        }

        public async Task ChangeBookData(Book book)
        {
            await Connection.ExecuteAsync(
                @"
                    UPDATE Book  
                        SET Description = @Description,
                        Name = @Name
                        WHERE BookId = @BookId
                ", book);
        }

        public async Task<ClientBook> GetClientBookOrDefault(int bookId, int clientId)
        {
            var res = await Connection.QuerySingleOrDefaultAsync<ClientBook>(@"
                SELECT TOP 1 * FROM ClientBook WHERE ClientId = @clientId AND BookId = @bookId ORDER BY WriteId DESC ", 
                new { bookId, clientId }
            );


            return res;
        }

        public async Task<IEnumerable<Book>> GetBooks(int page = 0)
        {
            const int limit = 10;
            int offset = page * limit;

            var res = await Connection.QueryAsync<Book>(@"
                SELECT * FROM Book ORDER BY DateCreated
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY
            ", new { limit, offset });

            return res;
        }

       

        public async Task<Book> GetBookOrDefault(int bookId)
        {
            var res = await Connection.QuerySingleAsync<Book>(@"
                SELECT * FROM Book WHERE BookId = @bookId
            ", new { bookId });

            return res;
        }
    }
}
