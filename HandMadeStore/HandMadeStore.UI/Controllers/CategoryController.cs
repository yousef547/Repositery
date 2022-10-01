using Microsoft.AspNetCore.Mvc;
using HandMadeStore.DataAccess;
using HandMadeStore.Models;
using HandMadeStore.Data;

namespace HandMadeStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> category = _context.Categories;
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
                var duplicatedProduct = _context.Categories

                    .FirstOrDefault(p => p.Name.ToLower() == category.Name.ToLower());
                if (duplicatedProduct != null)
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
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
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
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
            var productNameFromDb = _context.Categories.Find(category.Id).Name;
            if (!string.IsNullOrEmpty(category.Name))
            {
                var duplicatedProduct = _context.Categories

                    .FirstOrDefault(p => p.Name.ToLower() == category.Name.ToLower());
                if (duplicatedProduct != null && duplicatedProduct.Name.ToLower() != productNameFromDb.ToLower())
                {
                    //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
                    ModelState.AddModelError("name", "This product name is duplicated.");
                }
            }
            if (ModelState.IsValid)
            {
                _context.ChangeTracker.Clear();
                _context.Categories
   .Update(category);
                _context.SaveChanges();
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
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
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
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData.Add("success", "product deleted Successfully");
            return RedirectToAction("Index");
        }
    }
}
