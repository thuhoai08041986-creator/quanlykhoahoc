using CourseManagement.Data;
using CourseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CourseManagement.Controllers;
public class CategoriesController(AppDbContext db):Controller {
 public async Task<IActionResult> Index()=>View(await db.CourseCategories.Include(x=>x.Courses).AsNoTracking().OrderBy(x=>x.Id).ToListAsync());
 public IActionResult Create()=>View("Form",new CourseCategory());
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Create(CourseCategory c){if(!ModelState.IsValid)return View("Form",c);db.CourseCategories.Add(c);await db.SaveChangesAsync();TempData["Success"]="Đã thêm danh mục.";return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Edit(int id){var c=await db.CourseCategories.FindAsync(id);if(c==null)return Missing();return View("Form",c);}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Edit(int id,CourseCategory form){if(id!=form.Id)return Missing();var c=await db.CourseCategories.FindAsync(id);if(c==null)return Missing();if(!ModelState.IsValid)return View("Form",form);c.Name=form.Name;c.Description=form.Description;await db.SaveChangesAsync();TempData["Success"]="Đã sửa danh mục.";return RedirectToAction(nameof(Index));}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Delete(int id){var c=await db.CourseCategories.FindAsync(id);if(c==null)return Missing();if(await db.Courses.AnyAsync(x=>x.CourseCategoryId==id)){TempData["Error"]="Không thể xóa danh mục đang có khóa học.";return RedirectToAction(nameof(Index));}db.CourseCategories.Remove(c);try{await db.SaveChangesAsync();TempData["Success"]="Đã xóa danh mục.";}catch(DbUpdateException){TempData["Error"]="Không thể xóa vì danh mục đang được sử dụng.";}return RedirectToAction(nameof(Index));}
 IActionResult Missing(){Response.StatusCode=404;ViewBag.Message="Danh mục không tồn tại.";return View("~/Views/Shared/NotFoundPage.cshtml");}
}
