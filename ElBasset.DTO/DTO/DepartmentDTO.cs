using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم القسم")]
        public string Name { get; set; }
    }
}
