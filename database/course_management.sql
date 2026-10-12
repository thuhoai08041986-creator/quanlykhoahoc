IF DB_ID(N'CourseManagementDB') IS NULL CREATE DATABASE CourseManagementDB;
GO
USE CourseManagementDB;
GO
CREATE TABLE CourseCategories(Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) NOT NULL, Description NVARCHAR(MAX));
CREATE TABLE Courses(Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(200) NOT NULL, TuitionFee DECIMAL(18,2) NOT NULL CHECK(TuitionFee>0), Duration INT NOT NULL CHECK(Duration>0), IsActive BIT NOT NULL DEFAULT 1, Description NVARCHAR(MAX), CourseCategoryId INT NOT NULL, CONSTRAINT FK_Course_Category FOREIGN KEY(CourseCategoryId) REFERENCES CourseCategories(Id), CONSTRAINT UQ_Course_Category_Name UNIQUE(CourseCategoryId,Name));
CREATE TABLE CourseImages(Id INT IDENTITY(1,1) PRIMARY KEY, ImageUrl NVARCHAR(1000) NOT NULL, IsThumbnail BIT NOT NULL DEFAULT 0, CourseId INT NOT NULL, CONSTRAINT FK_Image_Course FOREIGN KEY(CourseId) REFERENCES Courses(Id) ON DELETE CASCADE);
GO
CREATE UNIQUE INDEX UX_OneThumbnailPerCourse ON CourseImages(CourseId) WHERE IsThumbnail=1;
GO
INSERT INTO CourseCategories(Name,Description) VALUES (N'Lập trình Web',N'Các khóa lập trình website'),(N'Dữ liệu',N'Phân tích dữ liệu'),(N'Thiết kế',N'Thiết kế đồ họa');
INSERT INTO Courses(Name,TuitionFee,Duration,IsActive,Description,CourseCategoryId) VALUES
(N'ASP.NET Core',2500000,45,1,N'Xây dựng web MVC',1),(N'ReactJS',1800000,28,1,N'Frontend React',1),(N'SQL Server',1400000,32,1,N'Cơ sở dữ liệu',2),(N'Python cơ bản',1200000,24,0,N'Python cho người mới',2),(N'Photoshop',900000,20,1,N'Chỉnh sửa ảnh',3);
INSERT INTO CourseImages(ImageUrl,IsThumbnail,CourseId) VALUES
('https://placehold.co/640x360/png?text=ASP.NET',1,1),('https://placehold.co/640x360/png?text=ASP.NET+2',0,1),('https://placehold.co/640x360/png?text=ReactJS',1,2),('https://placehold.co/640x360/png?text=SQL',1,3);
GO
