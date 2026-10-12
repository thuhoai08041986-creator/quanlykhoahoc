using System.ComponentModel.DataAnnotations;
namespace CourseManagement.Models;
public class CourseImage {
 public int Id {get;set;}
 [Required][StringLength(1000)][Url(ErrorMessage="URL ảnh không hợp lệ")] public string ImageUrl {get;set;}="";
 public bool IsThumbnail {get;set;}
 public int CourseId {get;set;}
 public Course? Course {get;set;}
}
