using Microsoft.AspNetCore.Mvc;
using HandMadeStore.DataAccess;
using HandMadeStore.Models;
using HandMadeStore.Data;
using HandMadeStore.DataAccess.Repository.IRepository;

namespace HandMadeStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> category = _unitOfWork.Category.GetAll();
            return View(category);
        }

        ////Create Product
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name))
            {
                var duplicatedProduct = _unitOfWork.Category.GetFirstOrDefault(p => p.Name.ToLower() == category.Name.ToLower());
                if (duplicatedProduct != null)
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData.Add("success", "product created Successfully");
                return RedirectToAction("Index");
            }
            return View(category);
        }

        ////Update Product
        //GET
        public IActionResult Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST
        [HttpPost]
        public IActionResult Update(Category category)
        {
            var productNameFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == category.Id).Name;
            if (!string.IsNullOrEmpty(category.Name))
            {
                var duplicatedProduct = _unitOfWork.Category
                    .GetFirstOrDefault(p => p.Name.ToLower() == category.Name.ToLower());
                if (duplicatedProduct != null && duplicatedProduct.Name.ToLower() != productNameFromDb.ToLower())
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.ClearChangeTrackin();
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData.Add("success", "product updated Successfully");

                return RedirectToAction("Index");
            }
            return View(category);
        }

        //GET
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetFirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData.Add("success", "product deleted Successfully");
            return RedirectToAction("Index");
        }
    }
}
