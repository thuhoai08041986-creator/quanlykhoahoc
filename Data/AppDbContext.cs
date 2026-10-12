using CourseManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace CourseManagement.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options) {
 public DbSet<Course> Courses=>Set<Course>();
 public DbSet<CourseCategory> CourseCategories=>Set<CourseCategory>();
 public DbSet<CourseImage> CourseImages=>Set<CourseImage>();
 protected override void OnModelCreating(ModelBuilder b){
 b.Entity<CourseCategory>().ToTable("CourseCategories");
 b.Entity<Course>().ToTable("Courses");
 b.Entity<CourseImage>().ToTable("CourseImages");
 b.Entity<Course>().Property(x=>x.TuitionFee).HasColumnType("decimal(18,2)");
 b.Entity<Course>().HasOne(x=>x.CourseCategory).WithMany(x=>x.Courses).HasForeignKey(x=>x.CourseCategoryId).OnDelete(DeleteBehavior.Restrict);
 b.Entity<CourseImage>().HasOne(x=>x.Course).WithMany(x=>x.CourseImages).HasForeignKey(x=>x.CourseId).OnDelete(DeleteBehavior.Cascade);
 b.Entity<Course>().HasIndex(x=>new{x.CourseCategoryId,x.Name}).IsUnique();
 b.Entity<CourseImage>().HasIndex(x=>x.CourseId).IsUnique().HasFilter("[IsThumbnail] = 1");
 }
}
