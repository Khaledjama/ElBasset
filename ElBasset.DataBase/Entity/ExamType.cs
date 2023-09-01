using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DataBase
{
    public class ExamType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please insert Exam Type Name")]
        public string ExamTypeName { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
