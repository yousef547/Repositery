using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace HandMadeStore.UI.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Brand> brand = _unitOfWork.Brand.GetAll();
            return View(brand);
        }

        ////Create Product
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!string.IsNullOrEmpty(brand.Name))
            {
                var duplicatedProduct = _unitOfWork.Brand.GetFirstOrDefault(p => p.Name.ToLower() == brand.Name.ToLower());
                if (duplicatedProduct != null)
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.Add(brand);
                _unitOfWork.Save();
                TempData.Add("success", "product created Successfully");
                return RedirectToAction("Index");

            }
            return View(brand);
        }

        ////Update Product
        //GET
        public IActionResult Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Brand.GetFirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST
        [HttpPost]
        public IActionResult Update(Brand brand)
        {
            var productNameFromDb = _unitOfWork.Brand.GetFirstOrDefault(x => x.Id == brand.Id).Name;
            if (!string.IsNullOrEmpty(brand.Name))
            {
                var duplicatedProduct = _unitOfWork.Brand
                    .GetFirstOrDefault(p => p.Name.ToLower() == brand.Name.ToLower());
                if (duplicatedProduct != null && duplicatedProduct.Name.ToLower() != productNameFromDb.ToLower())
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Brand.ClearChangeTrackin();
                _unitOfWork.Brand.Update(brand);
                _unitOfWork.Save();
                TempData.Add("success", "product updated Successfully");

                return RedirectToAction("Index");
            }
            return View(brand);
        }

        //GET
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var brand = _unitOfWork.Brand.GetFirstOrDefault(p => p.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var category = _unitOfWork.Brand.GetFirstOrDefault(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Brand.Remove(category);
            _unitOfWork.Save();
            TempData.Add("success", "product deleted Successfully");
            return RedirectToAction("Index");
        }
    }
}
