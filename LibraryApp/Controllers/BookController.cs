using System.Threading.Tasks;
using System.Web.Mvc;
using LibraryApp.Services.BookClient;
using LibraryApp.Services.Dtos;

namespace LibraryApp.Controllers
{
    public class BookController : Controller
    {
        readonly IBookService _bookService;
        public BookController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        
        
        public ActionResult Default()
        {
            return RedirectToAction("Index");
        }

        // GET: Book
        [Route("Book/{page?}")]
        public async Task<ActionResult> Index(int page = 0)
        {
            var books = await _bookService.GetBooks(page);

            return View(books);
        }

        // GET: Book
        [Route("Book/List/{page?}")]
        public async Task<ActionResult> List(int page = 0)
        {
            var books = await _bookService.GetBooks(page);

            return Json(books, JsonRequestBehavior.AllowGet);
        }


        // GET: Client/Details/5
        [Route("Book/Details/{bookId}")]
        public async Task<ActionResult> Details(int bookId)
        {
            var book = await _bookService.GetBook(bookId);

            return View(book);
        }

        // GET: Book/Create
        [Route("Book/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [Route("Book/Create")]
        [HttpPost]
        public async Task<ActionResult> Create(AddBookDto book)
        {
            try
            {
                var bookId = await _bookService.AddBook(book);

                return RedirectToAction("Details", new { bookId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        [Route("Book/Edit/{bookId}")]
        public async Task<ActionResult> Edit(int bookId)
        {
            var book = await _bookService.GetBook(bookId);
            var changeBookDto = new ChangeBookDataDto
            {
                BookId = book.BookId,
                Description = book.Description,
                Name = book.Name
            };
            return View(changeBookDto);
        }

        // POST: Book/Edit/5
        [Route("Book/Edit/{bookId}")]
        [HttpPost]
        public async Task<ActionResult> Edit(int bookId, ChangeBookDataDto book)
        {
            try
            {
                await _bookService.ChangeData(book);
                
                return RedirectToAction("Details", new { bookId });
            }
            catch
            {
                return View();
            }
        }


        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BookSearcher()
        {
            return PartialView();
        }
    }
}
