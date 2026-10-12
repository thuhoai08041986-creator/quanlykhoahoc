# Nâng cấp ảnh khóa học – EduManage

1. Mở thư mục này trong VS Code. Chạy `dotnet restore`, `dotnet build`, `dotnet run`.
2. SQL Server: `(.\SQLEXPRESS03)`; database `CourseManagementDB` phải có bảng `CourseImages`.
3. Trang **Khóa học** hiển thị ảnh đại diện hoặc ảnh đầu tiên, nếu không có ảnh sẽ ghi "Chưa có ảnh".
4. Vào **Chi tiết** để tải nhiều ảnh cùng lúc (tối đa 10 ảnh/lần, 5MB/ảnh) hoặc nhập URL.
5. Ảnh được lưu vào `wwwroot/uploads/courses`, URL lưu trong `CourseImages`.
6. Chọn **Chọn đại diện** để đổi ảnh; ảnh cũ tự bỏ trạng thái đại diện.
7. Form **Thêm / Sửa khóa học** cũng có ô chọn một ảnh từ máy.
8. Kết nối SQL trong `appsettings.json` phải khớp tên SQL Server máy bạn.

**Chú ý:** Đây là phiên bản ASP.NET Core MVC chạy trong VS Code, không phải Java Spring Boot. Chưa kiểm thử build tại môi trường hiện tại. Nên sao lưu dữ liệu và mã nguồn cũ trước khi thay.
