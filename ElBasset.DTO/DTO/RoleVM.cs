using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class RoleVM
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "لايجب ان يكون اسم الدور فارغ")]
        [StringLength(255, ErrorMessage = "يجب ان يكون اسم الدور بين 5 الي 255حرف", MinimumLength = 5)]
        public string RoleName { get; set; }
    }
}
