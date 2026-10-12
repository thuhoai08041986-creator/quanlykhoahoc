using CourseManagement.Data;
using CourseManagement.Models;
using CourseManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CourseManagement.Controllers;
public class CoursesController(AppDbContext db, IWebHostEnvironment env):Controller {
 async Task LoadCategories()=>ViewBag.Categories=new SelectList(await db.CourseCategories.OrderBy(x=>x.Name).ToListAsync(),"Id","Name");
 public async Task<IActionResult> Index([FromQuery]CourseSearch f){
  await LoadCategories();ViewBag.Filter=f;
  if(f.MinFee<0||f.MaxFee<0||(f.MinFee.HasValue&&f.MaxFee.HasValue&&f.MinFee>f.MaxFee)){ViewBag.Error="Khoảng học phí không hợp lệ: giá tối thiểu phải nhỏ hơn hoặc bằng giá tối đa.";return View(new List<Course>());}
  IQueryable<Course> q=db.Courses.AsNoTracking().Include(c=>c.CourseCategory).Include(c=>c.CourseImages);
  if(!string.IsNullOrWhiteSpace(f.Keyword))q=q.Where(c=>c.Name.Contains(f.Keyword.Trim()));
  if(f.CategoryId.HasValue)q=q.Where(c=>c.CourseCategoryId==f.CategoryId);
  if(f.IsActive.HasValue)q=q.Where(c=>c.IsActive==f.IsActive);
  if(f.MinFee.HasValue)q=q.Where(c=>c.TuitionFee>=f.MinFee);
  if(f.MaxFee.HasValue)q=q.Where(c=>c.TuitionFee<=f.MaxFee);
  q=f.SortBy switch {"fee_asc"=>q.OrderBy(c=>c.TuitionFee),"fee_desc"=>q.OrderByDescending(c=>c.TuitionFee),"duration_asc"=>q.OrderBy(c=>c.Duration),_=>q.OrderByDescending(c=>c.Id)};
  return View(await q.ToListAsync());
 }
 public async Task<IActionResult> Details(int id){var c=await db.Courses.Include(x=>x.CourseCategory).Include(x=>x.CourseImages).AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);if(c==null)return NotFoundPage("Khóa học không tồn tại.");return View(c);}
 public async Task<IActionResult> Create(){await LoadCategories();return View("Form",new Course());}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Create(Course c, IFormFile? photo){await ValidateCourse(c);if(!ModelState.IsValid){await LoadCategories();return View("Form",c);}db.Courses.Add(c);try{await db.SaveChangesAsync();if(photo!=null){var url=await SaveUploadedPhoto(photo);db.CourseImages.Add(new CourseImage{CourseId=c.Id,ImageUrl=url,IsThumbnail=true});await db.SaveChangesAsync();}}catch(InvalidOperationException ex){ModelState.AddModelError("",ex.Message);await LoadCategories();return View("Form",c);}catch(DbUpdateException){ModelState.AddModelError("","Không thể lưu khóa học. Vui lòng kiểm tra dữ liệu.");await LoadCategories();return View("Form",c);}TempData["Success"]="Đã thêm khóa học.";return RedirectToAction(nameof(Index));}
 public async Task<IActionResult> Edit(int id){var c=await db.Courses.FindAsync(id);if(c==null)return NotFoundPage("Khóa học không tồn tại.");await LoadCategories();return View("Form",c);}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Edit(int id,Course form,IFormFile? photo){if(id!=form.Id)return NotFoundPage("ID không hợp lệ.");var c=await db.Courses.FindAsync(id);if(c==null)return NotFoundPage("Khóa học không tồn tại.");await ValidateCourse(form);if(!ModelState.IsValid){await LoadCategories();return View("Form",form);}c.Name=form.Name.Trim();c.TuitionFee=form.TuitionFee;c.Duration=form.Duration;c.IsActive=form.IsActive;c.Description=form.Description;c.CourseCategoryId=form.CourseCategoryId;try{await db.SaveChangesAsync();if(photo!=null){var url=await SaveUploadedPhoto(photo);bool first=!await db.CourseImages.AnyAsync(x=>x.CourseId==id);db.CourseImages.Add(new CourseImage{CourseId=id,ImageUrl=url,IsThumbnail=first});await db.SaveChangesAsync();}}catch(InvalidOperationException ex){ModelState.AddModelError("",ex.Message);await LoadCategories();return View("Form",form);}catch(DbUpdateException){ModelState.AddModelError("","Không thể cập nhật khóa học.");await LoadCategories();return View("Form",form);}TempData["Success"]="Đã cập nhật khóa học.";return RedirectToAction(nameof(Index));}
 async Task ValidateCourse(Course c){if(!await db.CourseCategories.AnyAsync(x=>x.Id==c.CourseCategoryId))ModelState.AddModelError(nameof(c.CourseCategoryId),"Loại khóa học không tồn tại.");if(!string.IsNullOrWhiteSpace(c.Name)&&await db.Courses.AnyAsync(x=>x.CourseCategoryId==c.CourseCategoryId&&x.Name==c.Name.Trim()&&x.Id!=c.Id))ModelState.AddModelError(nameof(c.Name),"Tên khóa học đã tồn tại trong loại này.");}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> Delete(int id){var c=await db.Courses.FindAsync(id);if(c==null)return NotFoundPage("Khóa học không tồn tại.");db.Courses.Remove(c);await db.SaveChangesAsync();TempData["Success"]="Đã xóa khóa học.";return RedirectToAction(nameof(Index));}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> AddImage(int id,string imageUrl,bool isThumbnail=false){if(!await db.Courses.AnyAsync(x=>x.Id==id))return NotFoundPage("Khóa học không tồn tại.");if(string.IsNullOrWhiteSpace(imageUrl)||imageUrl.Length>1000||!Uri.TryCreate(imageUrl,UriKind.Absolute,out var uri)||(uri.Scheme!="https"&&uri.Scheme!="http")){TempData["Error"]="URL ảnh phải là địa chỉ http/https hợp lệ (tối đa 1000 ký tự).";return RedirectToAction(nameof(Details),new{id});}
  await using var tx=await db.Database.BeginTransactionAsync();try{var existing=await db.CourseImages.Where(x=>x.CourseId==id).ToListAsync();if(isThumbnail||existing.Count==0){foreach(var x in existing)x.IsThumbnail=false;await db.SaveChangesAsync();}db.CourseImages.Add(new CourseImage{CourseId=id,ImageUrl=imageUrl.Trim(),IsThumbnail=isThumbnail||existing.Count==0});await db.SaveChangesAsync();await tx.CommitAsync();TempData["Success"]="Đã thêm ảnh.";}catch(DbUpdateException){TempData["Error"]="Không thể thêm ảnh.";}return RedirectToAction(nameof(Details),new{id});}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> SetThumbnail(int id,int courseId){await using var tx=await db.Database.BeginTransactionAsync();var img=await db.CourseImages.FirstOrDefaultAsync(x=>x.Id==id&&x.CourseId==courseId);if(img==null)return NotFoundPage("Ảnh không tồn tại trong khóa học này.");await db.CourseImages.Where(x=>x.CourseId==courseId&&x.IsThumbnail).ExecuteUpdateAsync(s=>s.SetProperty(x=>x.IsThumbnail,false));await db.CourseImages.Where(x=>x.Id==id).ExecuteUpdateAsync(s=>s.SetProperty(x=>x.IsThumbnail,true));await tx.CommitAsync();TempData["Success"]="Đã đổi ảnh đại diện.";return RedirectToAction(nameof(Details),new{id=courseId});}
 [HttpPost][ValidateAntiForgeryToken]public async Task<IActionResult> DeleteImage(int id,int courseId){var img=await db.CourseImages.FirstOrDefaultAsync(x=>x.Id==id&&x.CourseId==courseId);if(img==null)return NotFoundPage("Ảnh không tồn tại.");db.CourseImages.Remove(img);await db.SaveChangesAsync();TempData["Success"]="Đã xóa ảnh.";return RedirectToAction(nameof(Details),new{id=courseId});}

 // Upload ảnh lên wwwroot/uploads/courses; database chỉ lưu URL tương đối.
 private async Task<string> SaveUploadedPhoto(IFormFile file){
  const long maxBytes=5*1024*1024;
  if(file.Length==0||file.Length>maxBytes)throw new InvalidOperationException("Ảnh phải có dung lượng từ 1 byte đến 5MB.");
  var ext=Path.GetExtension(file.FileName).ToLowerInvariant();
  if(!new[]{".jpg",".jpeg",".png",".webp",".gif"}.Contains(ext))throw new InvalidOperationException("Chỉ nhận ảnh JPG, PNG, WEBP hoặc GIF.");
  await using var input=file.OpenReadStream();
  var header=new byte[12];int read=await input.ReadAsync(header.AsMemory());
  bool jpg=read>=3&&header[0]==0xff&&header[1]==0xd8&&header[2]==0xff;
  bool png=read>=8&&header.Take(8).SequenceEqual(new byte[]{137,80,78,71,13,10,26,10});
  bool gif=read>=6&&System.Text.Encoding.ASCII.GetString(header,0,3)=="GIF";
  bool webp=read>=12&&System.Text.Encoding.ASCII.GetString(header,0,4)=="RIFF"&&System.Text.Encoding.ASCII.GetString(header,8,4)=="WEBP";
  if(!(jpg&&new[]{".jpg",".jpeg"}.Contains(ext)||png&&ext==".png"||gif&&ext==".gif"||webp&&ext==".webp"))throw new InvalidOperationException("Nội dung file không phải định dạng ảnh hợp lệ.");
  var folder=Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath,"wwwroot"),"uploads","courses");Directory.CreateDirectory(folder);
  var name=Guid.NewGuid().ToString("N")+ext;
  input.Seek(0,SeekOrigin.Begin);
  await using var output=System.IO.File.Create(Path.Combine(folder,name));await input.CopyToAsync(output);
  return "/uploads/courses/"+name;
 }
 [HttpPost][ValidateAntiForgeryToken]
 public async Task<IActionResult> UploadImage(int id,List<IFormFile>? photos,bool isThumbnail=false){
  if(!await db.Courses.AnyAsync(x=>x.Id==id))return NotFoundPage("Khóa học không tồn tại.");
  if(photos==null||photos.Count==0){TempData["Error"]="Vui lòng chọn ít nhất một ảnh.";return RedirectToAction(nameof(Details),new{id});}
  if(photos.Count>10){TempData["Error"]="Chỉ tải tối đa 10 ảnh mỗi lần.";return RedirectToAction(nameof(Details),new{id});}
  var savedUrls=new List<string>();
  try{
   // Lưu file trước, nếu có lỗi thì dọn các file đã lưu trong lần tải này.
   foreach(var photo in photos)savedUrls.Add(await SaveUploadedPhoto(photo));
   await using var tx=await db.Database.BeginTransactionAsync();
   bool first=!await db.CourseImages.AnyAsync(x=>x.CourseId==id);
   if(first||isThumbnail)await db.CourseImages.Where(x=>x.CourseId==id&&x.IsThumbnail).ExecuteUpdateAsync(s=>s.SetProperty(x=>x.IsThumbnail,false));
   for(int i=0;i<savedUrls.Count;i++)db.CourseImages.Add(new CourseImage{CourseId=id,ImageUrl=savedUrls[i],IsThumbnail=i==0&&(first||isThumbnail)});
   await db.SaveChangesAsync();await tx.CommitAsync();TempData["Success"]=$"Đã tải lên {savedUrls.Count} ảnh thành công.";
  }catch(Exception ex) when(ex is InvalidOperationException or DbUpdateException or IOException or UnauthorizedAccessException){
   foreach(var url in savedUrls){var path=Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath,"wwwroot"),url.TrimStart('/').Replace('/',Path.DirectorySeparatorChar));if(System.IO.File.Exists(path))System.IO.File.Delete(path);}
   TempData["Error"]=ex is InvalidOperationException?ex.Message:"Không thể lưu ảnh. Vui lòng thử lại.";
  }
  return RedirectToAction(nameof(Details),new{id});
 }
 IActionResult NotFoundPage(string message){Response.StatusCode=404;ViewBag.Message=message;return View("~/Views/Shared/NotFoundPage.cshtml");}
}
