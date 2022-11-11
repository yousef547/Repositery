using Microsoft.AspNetCore.Mvc;
using HandMadeStore.DataAccess;
using HandMadeStore.Models;
using HandMadeStore.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using HandMadeStore.Model.Models.ViewModel;

namespace HandMadeStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _host;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _host = host;
        }

        public IActionResult Index()
        {
            return View();
        }

        ////Create Product
        //GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        ////POST
        //[HttpPost]
        //public IActionResult Create(Product product)
        //{
        //    if (!string.IsNullOrEmpty(product.Name))
        //    {
        //        var duplicatedProduct = _unitOfWork.Product.GetFirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
        //        if (duplicatedProduct != null)
        //        {
        //            //ModelState.AddModelError(String.Empty, "This product name is duplicated.");
        //            ModelState.AddModelError("name", "This product name is duplicated.");
        //        }
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(product);
        //        _unitOfWork.Save();
        //        TempData.Add("success", "product created Successfully");
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        ////Update Product
        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                ),
                BrandList = _unitOfWork.Brand.GetAll().Select(
                c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }
                )
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                return View(productVM);

            }
        }

        //POST
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile file) 
        {
            if (ModelState.IsValid)
            {
                string RootPath = _host.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var productsFolderPath = Path.Combine(RootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if(productVM.Product.ImageUrl != null)
                    {
                        var pathImage = Path.Combine(RootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(pathImage))
                        {
                            System.IO.File.Delete(pathImage);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(productsFolderPath,
                        fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (productVM.Product.Id == 0)
                {
                    //Create new product
                    _unitOfWork.Product.Add(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    //Update product
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }

            }
            return View(productVM);
        }

   
   
        #region API EndPoints
        public IActionResult GetAllProducts()
        {
            string[] arr = { "Category", "Brand"};
            var products = _unitOfWork.Product.GetAll(arr);
            return Json(new
            {
                data = products
            });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return Json(new {success=false,message="Error while deleting product"});
            }
            string RootPath = _host.WebRootPath;
            var pathImage = Path.Combine(RootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "product delete succsessfully" });

        }

        #endregion
    }
}
