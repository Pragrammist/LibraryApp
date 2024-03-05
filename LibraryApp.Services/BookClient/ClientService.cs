using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.DataAccess.Models;
using LibraryApp.DataAccess.Repositories;
using LibraryApp.Services.Dtos;
using LibraryApp.Services.Dtos.Book;
using LibraryApp.Services.Dtos.Client;
using LibraryApp.Services.Exceptions;

namespace LibraryApp.Services.BookClient
{

    public interface IClientService
    {
        Task<int> AddBook(int clientId, int bookId);
        Task<int> AddClient(AddClientDto addClientDto);
        Task ChangeClientData(ChangeClientDataDto client);
        Task<ClientDatailsDto> GetClient(int clientId);
        Task<IEnumerable<BookWithStatusDto>> GetClientBooks(int clientId);
        Task<IEnumerable<ClientDto>> GetClients(int page = 0);
        Task<int> RemoveBook(int clientId, int bookId);
    }
    public class ClientService : IClientService
    {

        readonly IClientBookUnitOfWork _clientBookUnitOfWork;
        public ClientService(IClientBookUnitOfWork clientBookUnitOfWork)
        {
            _clientBookUnitOfWork = clientBookUnitOfWork;
        }

        public async Task<int> AddClient(AddClientDto addClientDto)
        {
            var res = await _clientBookUnitOfWork.ClientRepository.AddClient(new DataAccess.Models.Client
            {
                Address = addClientDto.Address,
                Name = addClientDto.Name,
                Telephone = addClientDto.Telephone
            });


            _clientBookUnitOfWork.Close();

            return res;
        }

        public async Task<int> AddBook(int clientId, int bookId)
        {
            var bookStatus = await _clientBookUnitOfWork.BookRepository.GetClientBookOrDefault(clientId, bookId);
            
            if (!(bookStatus is null) && bookStatus.Status == 1)
                throw new NotValidActionException("Клиент уже взял эту книгу!");
            
            var res = await _clientBookUnitOfWork.ClientRepository.AddBookToClient(clientId, bookId);
            
            _clientBookUnitOfWork.Close();
            return res;
        }

        public async Task<int> RemoveBook(int clientId, int bookId)
        {
            var bookStatus = await _clientBookUnitOfWork.BookRepository.GetClientBookOrDefault(clientId, bookId);

            if (bookStatus is null || bookStatus.Status == 0)
                throw new NotValidActionException("Клиент не брал эту книгу!");

            var res = await _clientBookUnitOfWork.ClientRepository.RemoveBookFromClient(clientId, bookId);



            _clientBookUnitOfWork.Close();
            return res;
        }

        public async Task<IEnumerable<ClientDto>> GetClients(int page = 0)
        {
            var clients = await _clientBookUnitOfWork.ClientRepository.GetClients(page);

            _clientBookUnitOfWork.Close();
            
            return clients.Select(s => new ClientDto { ClientId = s.ClientId, Name = s.Name, Telephone = s.Telephone });
        }

        public async Task<ClientDatailsDto> GetClient(int clientId)
        {
            var client = await _clientBookUnitOfWork.ClientRepository.GetClientOrDefault(clientId) 
                   ?? throw new NotValidActionException("Клиент не найден");

            
            _clientBookUnitOfWork.Close();
            
            return new ClientDatailsDto
            {
                Telephone = client.Telephone,
                ClientId = client.ClientId,
                Name = client.Name,
                Address = client.Address,
                DateCreated = client.DateCreated
            };
        }
        public async Task <IEnumerable<BookWithStatusDto>> GetClientBooks(int clientId)
        {
            var books = await _clientBookUnitOfWork.ClientRepository.GetClientBooks(clientId);

            return books.Select(s => new BookWithStatusDto
            {
                BookId = s.BookId,
                Name = s.Name,
                Status = s.Status,
                WriteDate = s.WriteDate,
                
            });
        }

        public async Task ChangeClientData(ChangeClientDataDto client)
        {
            await _clientBookUnitOfWork.ClientRepository.ChangeClientData(new Client
            {
                Address = client.Address,
                ClientId = client.ClientId,
                Name = client.Name,
                Telephone= client.Telephone
            });
        }
    }
}
