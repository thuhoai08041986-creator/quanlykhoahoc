# Course Management - Visual Studio Community 2022

## Công nghệ
ASP.NET Core MVC (.NET 8), Entity Framework Core 8, SQL Server, Bootstrap 5.

## Database First
1. Mở SSMS, chạy `database/course_management.sql` để tạo ba bảng và dữ liệu mẫu (chỉ chạy lần đầu).
2. Mở `CourseManagement.csproj` bằng Visual Studio 2022 (cần workload ASP.NET and web development và .NET 8 SDK).
3. Sửa `appsettings.json` với SQL Server instance của máy. Mẫu `Server=.\SQLEXPRESS` có thể thay bằng `Server=LAPTOP-...\SQLEXPRESS04`.
4. Restore NuGet packages, chọn F5 để chạy. Trang chủ /, danh sách /Courses, danh mục /Categories.
5. Không chạy `Add-Migration` hoặc `Update-Database`. CSDL được tạo bằng SQL trước.

## Chức năng
Dashboard, CRUD khóa học và danh mục, lọc đồng thời ở SQL qua IQueryable, phân loại thời lượng không lưu cột, thêm/xóa ảnh URL, đổi ảnh đại diện trong transaction, xử lý ID không tồn tại, kiểm tra tên trùng, chặn xóa danh mục có khóa học.

## Lưu ý
Đây là phiên bản C# cho Visual Studio Community 2022, **không phải Java Spring Boot**. Chưa kiểm thử trực tiếp trên SQL Server của người dùng. CDN Bootstrap/Google Fonts cần internet.
