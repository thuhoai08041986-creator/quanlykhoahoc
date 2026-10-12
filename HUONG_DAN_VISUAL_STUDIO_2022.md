# Chạy trên Visual Studio Community 2022

1. Cài workload **ASP.NET and web development** qua Visual Studio Installer và .NET 8 SDK.
2. Mở `CourseManagement.sln` (không mở file HTML riêng).
3. Mở `appsettings.json`: mặc định `Server=.\\SQLEXPRESS03;Database=CourseManagementDB;Trusted_Connection=True`.
4. Kiểm tra database `CourseManagementDB` có đủ `CourseCategories`, `Courses`, `CourseImages` trong SSMS. Nếu chưa có, chạy `database/course_management.sql` trên đúng instance.
5. Nhấn Ctrl+Shift+B, chọn profile **https** hoặc **http** và F5.
6. Vào `/Courses/Create` để tạo khóa học và chọn 1 ảnh; vào `/Courses/Details/1` để tải nhiều ảnh, thêm URL, đổi ảnh đại diện và xóa ảnh.
7. Ảnh tải lên lưu tại `wwwroot/uploads/courses`; bảng `CourseImages` lưu đường dẫn.

Chú ý: Đây là ASP.NET Core MVC C# (không phải Java Spring Boot). Chưa được build kiểm thử với SQL Server trên máy Windows.
