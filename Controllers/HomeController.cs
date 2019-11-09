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

        public async Task<IActionResult> CreateOrUpdate(
            string database,
            string collection,
            string id,
            int index,
            string fieldName,
            string value
        )
        {
            await _documentService.CreateOrUpdate(database, collection, id, fieldName, value);
            return RedirectToAction("Index", GetRouteValues(database, collection, index));
        }

        public async Task<IActionResult> Delete(
            string database,
            string collection,
            string id,
            int index
        )
        {
            await _documentService.Delete(database, collection, id);
            return RedirectToAction("Index", GetRouteValues(database, collection, index));
        }

        private static object GetRouteValues(string database, string collection, int index)
        {
            return new { selectedDatabase = database, selectedCollection = collection, index = index };
        }


        public async Task<IActionResult> Index(string selectedDatabase, string selectedCollection, int index = 0)
        {
            var databasesAndCollections = await _documentService.GetDatabasesAndCollections();
            var viewModel = new ExplorerDbViewModel()
            {
                DatabasesAndCollections = databasesAndCollections,
                Database = selectedDatabase,
                Collection = selectedCollection,
                Index = index
            };
            if (selectedCollection != null && selectedDatabase != null)
            {
                viewModel.Document = await _documentService.GetRow(selectedDatabase, selectedCollection, index);
                viewModel.CollectionCount = await _documentService.GetCollectionCount(selectedDatabase, selectedCollection);
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
