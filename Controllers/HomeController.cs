using CourseManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CourseManagement.Controllers;
public class HomeController(AppDbContext db):Controller {
 public async Task<IActionResult> Index(){ViewBag.Courses=await db.Courses.CountAsync();ViewBag.Categories=await db.CourseCategories.CountAsync();ViewBag.Active=await db.Courses.CountAsync(c=>c.IsActive);ViewBag.Inactive=await db.Courses.CountAsync(c=>!c.IsActive);return View(await db.Courses.Include(c=>c.CourseCategory).OrderByDescending(c=>c.Id).Take(5).ToListAsync());}
 public IActionResult Error()=>View("~/Views/Shared/Error.cshtml");
}
