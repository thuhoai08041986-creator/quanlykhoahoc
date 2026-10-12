using System.ComponentModel.DataAnnotations;
namespace CourseManagement.Models;
public class CourseCategory {
 public int Id {get;set;}
 [Required(ErrorMessage="Tên loại không được để trống")][StringLength(150)] public string Name {get;set;}="";
 public string? Description {get;set;}
 public ICollection<Course> Courses {get;set;}=new List<Course>();
}
