# CourseManagement — Visual Studio Code + SQL Server

## Công nghệ
ASP.NET Core MVC (.NET 8), Entity Framework Core 8, Razor, SQL Server. **Không phải Java Spring Boot.**

## Chuẩn bị
1. Cài .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
2. Cài Visual Studio Code: https://code.visualstudio.com/
3. Trong VS Code cài `C# Dev Kit` và `C#` (Microsoft).
4. Mở thư mục `CourseManagement_VSCode` (thư mục chứa `CourseManagement.csproj`).
5. Mở SSMS, kết nối `.\SQLEXPRESS03` bằng Windows Authentication. Nếu chưa có database, chạy `database/course_management.sql` trên đúng instance. Nếu đã có database, kiểm tra schema trước khi chạy script để tránh tạo trùng.

## Kết nối SQL Server
`appsettings.json` đã thiết lập:

`Server=.\SQLEXPRESS03;Database=CourseManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;`

Nếu tên instance trên máy thay đổi, sửa trong `appsettings.json`.

## Chạy trong VS Code
Mở Terminal > New Terminal:

```powershell
dotnet --version
dotnet restore
dotnet build
dotnet run --urls http://localhost:5080
```

Truy cập http://localhost:5080. Có thể nhấn F5 để debug bằng `.vscode/launch.json` hoặc Terminal > Run Task > run.

## Lưu ý
- Database-first: các bảng SQL được tạo bởi script, ứng dụng không gọi `EnsureCreated` hoặc `Migrate`.
- Không cần cài JDK, Maven hoặc Spring Boot để chạy bản C# này.
- Chưa được xác minh build trên máy có .NET SDK/SQL Server của người dùng.
