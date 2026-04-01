using Microsoft.AspNetCore.Mvc;
using Dhungbai7.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dhungbai7.Controllers
{
    public class ProductsController : Controller
    {
        // QUAN TRỌNG: Phải dùng 'static' để danh sách tồn tại suốt quá trình chạy app
        private static List<Product> _products = new List<Product>
{
    new Product { Id = 1, Name = "laptop max", Price = 999999, Category = "laptop max" },
    new Product { Id = 2, Name = "iphone 11", Price = 9999, Category = "laptop " },
    new Product { Id = 3, Name = "iphone 15 pro", Price = 9999, Category = "nokia " },
    new Product { Id = 4, Name = "pc", Price = 99999999, Category = "laptop " },
    new Product { Id = 5, Name = "hoa hong den", Price = 99999999, Category = "Rose " },
    // Sản phẩm thứ 6 này sẽ nằm ở Trang 2
    new Product { Id = 6, Name = "pc LOQ", Price = 999999999, Category = "laptop " }
};
        public IActionResult Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 5; // Số lượng sản phẩm trên mỗi trang
            var products = _products.AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Tính toán phân trang
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy dữ liệu của trang hiện tại
            var items = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Gửi dữ liệu phân trang sang View qua ViewBag
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;

            return View(items);
        }


        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Tự động tăng ID cho sản phẩm mới
                product.Id = _products.Max(p => p.Id) + 1;

                // THÊM SẢN PHẨM VÀO DANH SÁCH
                _products.Add(product);

                // Chuyển về trang danh sách để xem kết quả
                return RedirectToAction("Index");
            }
            return View(product);
        }
        // --- DETAILS ---
        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // --- EDIT (GET: Hiển thị form) ---
        public IActionResult Edit(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // --- EDIT (POST: Lưu thay đổi) ---
        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Category = product.Category;
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // --- DELETE (GET: Xác nhận xóa) ---
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // --- DELETE (POST: Thực hiện xóa) ---
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return RedirectToAction("Index");
        }

    }
}