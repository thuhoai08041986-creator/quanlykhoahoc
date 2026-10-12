using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CourseManagement.Models;
public class Course {
 public int Id {get;set;}
 [Required(ErrorMessage="Tên khóa học không được để trống")][StringLength(200)] public string Name {get;set;}="";
 [Range(typeof(decimal),"0.01","999999999999",ErrorMessage="Học phí phải lớn hơn 0")] public decimal TuitionFee {get;set;}
 [Range(1,int.MaxValue,ErrorMessage="Thời lượng phải lớn hơn 0")] public int Duration {get;set;}
 public bool IsActive {get;set;}=true;
 public string? Description {get;set;}
 [Range(1,int.MaxValue,ErrorMessage="Hãy chọn loại khóa học")] public int CourseCategoryId {get;set;}
 public CourseCategory? CourseCategory {get;set;}
 public ICollection<CourseImage> CourseImages {get;set;}=new List<CourseImage>();
 [NotMapped] public string DurationType => Duration<=30?"Ngắn hạn":"Dài hạn";
}
