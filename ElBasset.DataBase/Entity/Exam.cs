using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DataBase
{
    public class Exam
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please insert Exam Name")]
        public string ExamName { get; set; }
        [Required(ErrorMessage = "Please insert Exam Link ")]
        public string ExamLink { get; set; }
        [Required(ErrorMessage = "Please insert Start Date Time of Exam ")]
        public DateTime StartDateTimeExam { get; set; }
        [Required(ErrorMessage = "Please insert End Date Time of Exam ")]
        public DateTime EndDateTimeExam { get; set; }
        public bool Statues { get; set; }

        public int ExamTypeId { get; set; }
        public virtual ExamType ExamType { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

    }
}
