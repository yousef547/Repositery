using Microsoft.AspNetCore.Mvc;
using HandMadeStore.DataAccess;
using HandMadeStore.Models;
using HandMadeStore.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HandMadeStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> product = _unitOfWork.Product.GetAll();
            return View(product);
        }

        ////Create Product
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!string.IsNullOrEmpty(product.Name))
            {
                var duplicatedProduct = _unitOfWork.Product.GetFirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                if (duplicatedProduct != null)
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                TempData.Add("success", "product created Successfully");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        ////Update Product
        //GET
        public IActionResult Upsert(int? id)
        {
            Product product = new();

            IEnumerable<SelectListItem> CatogeryList = _unitOfWork.Category.GetAll().Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                );

            IEnumerable<SelectListItem> BrandList = _unitOfWork.Brand.GetAll().Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                );
            if (id == null || id == 0)
            {
                ViewBag.CatogeryList = CatogeryList;
                ViewBag.BrandList = BrandList;

                return View(product);
            }
            else
            {
                product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                return View(product);

            }
        }

        //POST
        [HttpPost]
        public IActionResult Update(Product product)
        {
            var productNameFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == product.Id).Name;
            if (!string.IsNullOrEmpty(product.Name))
            {
                var duplicatedProduct = _unitOfWork.Product
                    .GetFirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                if (duplicatedProduct != null && duplicatedProduct.Name.ToLower() != productNameFromDb.ToLower())
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.ClearChangeTrackin();
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData.Add("success", "product updated Successfully");

                return RedirectToAction("Index");
            }
            return View(product);
        }

        //GET
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData.Add("success", "product deleted Successfully");
            return RedirectToAction("Index");
        }
    }
}
