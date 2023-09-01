using ElBasset.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class ExamDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم الامتحان")]
        public string ExamName { get; set; }
        [Required(ErrorMessage = "من فضل ادخل لينك الامتحان ")]
        public string ExamLink { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل تاريخ بداية الامتحان ")]
        public DateTime StartDateTimeExam { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل تاريخ نهاية الامتحان ")]
        public DateTime EndDateTimeExam { get; set; }
        public bool Statues { get; set; }
        public int ExamTypeId { get; set; }
        public virtual ExamType? ExamType { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
