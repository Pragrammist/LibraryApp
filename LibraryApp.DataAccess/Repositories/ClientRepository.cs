

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.UI;
using Dapper;
using LibraryApp.DataAccess.Models;

namespace LibraryApp.DataAccess.Repositories
{

    public interface IClientRepository
    {
        Task<int> AddBookToClient(int clientId, int bookId);
        Task<int> AddClient(Client client);
        Task ChangeClientData(Client client);
        Task<IEnumerable<BookWithStatus>> GetClientBooks(int clientId);
        Task<Client> GetClientOrDefault(int clientId);
        Task<IEnumerable<Client>> GetClients(int page = 0);
        Task<int> RemoveBookFromClient(int clientId, int bookId);
    }

    public class ClientRepository : IClientRepository
    {
        readonly IConnectionFactory _connectionFactory;

        IDbConnection Connection => _connectionFactory.GetDbConnection();
        public ClientRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddClient(Client client)
        {
            var res = await Connection.QuerySingleAsync<int>(
                @"
                    INSERT INTO Client (Name, Telephone, Address) 
                    VALUES (@Name, @Telephone, @Address);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ", client);

            return res;
        }

        public async Task<int> AddBookToClient(int clientId, int bookId)
        {
            //если пользователь берет книгу, то ставится 1(дефолтное значение) в колонку таблицы status
            var res = await Connection.QuerySingleAsync<int>(
                @"
                    INSERT INTO ClientBook (BookId, ClientId) 
                    VALUES (@bookId, @clientId);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ", new {bookId, clientId});

            return res;
        }

        public async Task<int> RemoveBookFromClient(int clientId, int bookId)
        {
            //если пользователь берет книгу, то ставится 0 в колонку таблицы status
            var status = 0;
            var res = await Connection.QuerySingleAsync<int>(
                @"
                    INSERT INTO ClientBook (BookId, ClientId, Status) 
                    VALUES (@bookId, @clientId, @status);
                    SELECT CAST(SCOPE_IDENTITY() as int);
                ", new { bookId, clientId, status});

            return res;
        }

        public async Task<IEnumerable<Client>> GetClients(int page = 0)
        {
            const int limit = 10;
            int offset = page * limit;
            
            var res = await Connection.QueryAsync<Client>(@"
                SELECT * FROM Client ORDER BY DateCreated
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY
            ", new { limit, offset});

            return res;
        }

        public async Task<Client> GetClientOrDefault(int clientId)
        {
            var res = await Connection.QuerySingleAsync<Client>(@"
                SELECT * FROM Client WHERE ClientId = @clientId
            ", new { clientId });

            return res;
        }

        public async Task<IEnumerable<BookWithStatus>> GetClientBooks(int clientId)
        {
            var res = await Connection.QueryAsync<BookWithStatus>(
                @"
                    SELECT TOP 3 Book.*, ClientBook.Status, ClientBook.WriteDate FROM Book 
                    INNER JOIN ClientBook ON ClientId = @clientId
                    ORDER BY ClientBook.WriteId DESC
                ", new  { clientId});

            return res;
        }

        public async Task ChangeClientData(Client client)
        {
            await Connection.ExecuteAsync(
                @"
                    UPDATE Client  
                        SET Telephone = @Telephone,
                        Name = @Name,
                        Address = @Address
                        WHERE ClientId = @ClientId
                ", client);
        }
    }
}
