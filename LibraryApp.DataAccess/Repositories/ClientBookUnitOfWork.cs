

namespace LibraryApp.DataAccess.Repositories
{
    public interface IClientBookUnitOfWork
    {
        IBookRepository BookRepository { get; }

        IClientRepository ClientRepository { get; }

        void Close();
    }
    public class ClientBookUnitOfWork : IClientBookUnitOfWork
    {
        readonly IBookRepository _bookRepository;
        public IBookRepository BookRepository => _bookRepository;

        readonly IClientRepository _clientRepository;
        public IClientRepository ClientRepository => _clientRepository;

        readonly IConnectionFactory _connectionFactory;

        public ClientBookUnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            _bookRepository = new BookRepository(_connectionFactory);

            _clientRepository = new ClientRepository(_connectionFactory);
        }

        public void Close()
        {
            _connectionFactory.Close();
        }
    }
}
