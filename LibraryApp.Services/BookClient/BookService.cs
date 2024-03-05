using System.Threading.Tasks;
using LibraryApp.DataAccess.Repositories;
using LibraryApp.Services.Dtos;
using LibraryApp.DataAccess.Models;
using LibraryApp.Services.Dtos.Book;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Services.BookClient
{

    public interface IBookService
    {
        Task<int> AddBook(AddBookDto book);
        Task ChangeData(ChangeBookDataDto changeBookDataDto);
        Task<BookDetailsDto> GetBook(int bookId);
        Task<IEnumerable<BookDto>> GetBooks(int page = 0);
    }
    public class BookService : IBookService
    {
        readonly IClientBookUnitOfWork _clientBookUnitOfWork;
        public BookService(IClientBookUnitOfWork clientBookUnitOfWork)
        {
            _clientBookUnitOfWork = clientBookUnitOfWork;
        }

        public async Task<int> AddBook(AddBookDto book)
        {
            var res = await _clientBookUnitOfWork.BookRepository.AddBook(new Book
            {
                Description = book.Description,
                Name = book.Name,
            });



            _clientBookUnitOfWork.Close();

            return res;
        }

        public async Task ChangeData(ChangeBookDataDto changeBookDataDto)
        {
            await _clientBookUnitOfWork.BookRepository.ChangeBookData(new Book
            {
                BookId = changeBookDataDto.BookId,
                Description = changeBookDataDto.Description,
                Name = changeBookDataDto.Name,
            });


            _clientBookUnitOfWork.Close();
        }

        public async Task<IEnumerable<BookDto>> GetBooks(int page = 0)
        {
            var res = await _clientBookUnitOfWork.BookRepository.GetBooks(page);

            _clientBookUnitOfWork.Close();
            
            return res.Select(s => new BookDto { BookId = s.BookId, Name = s.Name });
            
        }

        public async Task<BookDetailsDto> GetBook(int bookId)
        {
            var res = await _clientBookUnitOfWork.BookRepository.GetBookOrDefault(bookId);

            _clientBookUnitOfWork.Close();

            return new BookDetailsDto
            {
                Name = res.Name,
                BookId = res.BookId,
                DateCreated = res.DateCreated,
                Description = res.Description
            };

        }
    }
}
