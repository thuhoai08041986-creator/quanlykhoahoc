# Cập nhật chức năng ảnh khóa học

- Create/Edit: chọn ảnh trực tiếp từ máy tính (không bắt buộc).
- Details: tải ảnh từ máy tính hoặc thêm URL; xem tất cả ảnh; đổi ảnh đại diện; xóa ảnh.
- File tải lên nằm tại `wwwroot/uploads/courses/`; SQL Server chỉ lưu đường dẫn trong `CourseImages.ImageUrl`.
- Giới hạn 5MB; hỗ trợ JPG/PNG/WEBP/GIF; kiểm tra phần mở đầu file.
- Không cần chạy migration hoặc thay đổi ba bảng SQL Server.
- Kết nối `.\SQLEXPRESS03` trong appsettings.json.

Chạy `dotnet restore`, `dotnet build`, `dotnet run`.
