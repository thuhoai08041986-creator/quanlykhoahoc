USE CourseManagementDB;
GO
-- Chạy sau khi đã tạo bảng và dữ liệu khóa học. Có thể chạy lại nhiều lần.
-- Ảnh được lưu sẵn trong wwwroot/images/courses, không phụ thuộc Internet.
SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @photos TABLE (CourseName NVARCHAR(200), ImageUrl NVARCHAR(1000));
INSERT INTO @photos VALUES
(N'ASP.NET Core',N'/images/courses/aspnet.svg'),
(N'ReactJS',N'/images/courses/react.svg'),
(N'SQL Server',N'/images/courses/sql.svg'),
(N'Python cơ bản',N'/images/courses/python.svg'),
(N'Photoshop',N'/images/courses/photoshop.svg'),
(N'lap trinh c#',N'/images/courses/csharp.svg');
-- Chỉ tác động đến khóa học có tên khớp, không xóa ảnh do người dùng thêm.
UPDATE ci SET IsThumbnail=0
FROM dbo.CourseImages ci JOIN dbo.Courses c ON c.Id=ci.CourseId
JOIN @photos p ON p.CourseName=c.Name
WHERE ci.IsThumbnail=1
AND NOT EXISTS (SELECT 1 FROM dbo.CourseImages x WHERE x.CourseId=c.Id AND x.ImageUrl=p.ImageUrl);
INSERT INTO dbo.CourseImages(CourseId,ImageUrl,IsThumbnail)
SELECT c.Id,p.ImageUrl,1
FROM dbo.Courses c JOIN @photos p ON p.CourseName=c.Name
WHERE NOT EXISTS (SELECT 1 FROM dbo.CourseImages ci WHERE ci.CourseId=c.Id AND ci.ImageUrl=p.ImageUrl);
-- Nếu đã thêm trước đó nhưng chưa đặt làm đại diện, giữ nguyên đại diện hiện có.
UPDATE ci SET IsThumbnail=1
FROM dbo.CourseImages ci JOIN dbo.Courses c ON c.Id=ci.CourseId
JOIN @photos p ON p.CourseName=c.Name AND ci.ImageUrl=p.ImageUrl
WHERE NOT EXISTS (SELECT 1 FROM dbo.CourseImages other WHERE other.CourseId=c.Id AND other.IsThumbnail=1);
COMMIT;
SELECT c.Name,ci.ImageUrl,ci.IsThumbnail FROM dbo.Courses c JOIN dbo.CourseImages ci ON ci.CourseId=c.Id ORDER BY c.Id,ci.IsThumbnail DESC;
GO
