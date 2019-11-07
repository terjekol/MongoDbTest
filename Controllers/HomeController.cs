using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDbTest.Models;
using MongoDbTest.Services;

namespace MongoDbTest.Controllers
{
    public class HomeController : Controller
    {
        private DocumentService _documentService;

        public HomeController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<IActionResult> Index(string selectedDatabase, string selectedCollection)
        {
            var databasesAndCollections = await _documentService.GetDatabasesAndCollections();
            var viewModel = new ExplorerDbViewModel()
            {
                DatabasesAndCollections = databasesAndCollections,
                SelectedDatabase = selectedDatabase,
                SelectedCollection = selectedCollection
            };
            if (selectedCollection != null && selectedDatabase != null)
            {
                viewModel.Documents = await _documentService.GetRows(selectedDatabase, selectedCollection);
            }
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
