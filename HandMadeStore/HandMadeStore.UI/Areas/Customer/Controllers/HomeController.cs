using Microsoft.AspNetCore.Mvc;
using HandMadeStore.Models;
using System.Diagnostics;
using HandMadeStore.UI.Models;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.DataAccess.Repository;
using HandMadeStore.Model.Models;

namespace HandMadeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _iunitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _iunitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string[] arr = { "Category", "Brand" };
            IEnumerable<Product> products = _iunitOfWork.Product.GetAll(arr);
            return View(products);
        }

        public IActionResult Details(int id)
        {
            CardItem cartItem = new()
            {
                Count = 1,
                ProductId = id,
                Product = _iunitOfWork.Product.GetFirstOrDefault(p => p.Id == id, inCludeProperties: "Category,Brand")
            };
            return View(cartItem);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}