using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibraryApp.Services.BookClient;
using LibraryApp.Services.Dtos;
using LibraryApp.Services.Dtos.Client;

namespace LibraryApp.Controllers
{
    public class ClientController : Controller
    {
        readonly IClientService _clientService;
        public ClientController(IClientService clientService) 
        {
            _clientService = clientService;
        }

        public ActionResult Default()
        {
            return RedirectToAction("Index");
        }

        // GET: Client
        [Route("Client/{page?}")]
        public async Task<ActionResult> Index(int page = 0)
        {
            var clients = await _clientService.GetClients(page);
            return View(clients);
        }

        // GET: Client/Details/5
        [Route("Client/Details/{clientId}")]
        public async Task<ActionResult> Details(int clientId)
        {
            var client = await _clientService.GetClient(clientId);

            return View(client);
        }

        // GET: Client/Create
        [Route("Client/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Client/Book/Add")]
        [HttpPost]
        public async Task<ActionResult> AddBookToClient(int clientId, int bookId)
        {
            await _clientService.AddBook(clientId, bookId);

            return Json(new { });
        }

        [Route("Client/Book/Delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteBookFromClient(int clientId, int bookId)
        {
            await _clientService.RemoveBook(clientId, bookId);

            return Json(new { });
        }

        // POST: Client/Create
        [Route("Client/Create")]
        [HttpPost]
        public async Task<ActionResult> Create(AddClientDto client)
        {
            try
            {
                var clientId = await _clientService.AddClient(client);

                return RedirectToAction("Details", new { clientId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Edit/5
        [Route("Client/Edit/{clientId}")]
        public async Task<ActionResult> Edit(int clientId)
        {
            var client = await _clientService.GetClient(clientId);

            var clientChangeData = new ChangeClientDataDto
            {
                Address = client.Address,
                ClientId = client.ClientId,
                Name = client.Name,
                Telephone = client.Telephone
            };
            return View(clientChangeData);
        }

        // POST: Client/Edit/5
        [Route("Client/Edit/{clientId}")]
        [HttpPost]
        public async Task<ActionResult> Edit(int clientId, ChangeClientDataDto client)
        {
            try
            {
                await _clientService.ChangeClientData(client);

                return RedirectToAction("Details", new { clientId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var clientId = id;

                return RedirectToAction("Details", new { clientId });
            }
            catch
            {
                return View();
            }
        }

        [Route("Client/Book/ShowList")]
        [HttpGet]
        public async Task<ActionResult> ShowClientBook(int clientId)
        {
            ViewBag.clientId = clientId;
            var clientBooks = await _clientService.GetClientBooks(clientId);

            return PartialView("_ShowClientBook", clientBooks);
        }
    }
}
