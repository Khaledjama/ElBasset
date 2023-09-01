using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "لا يجب ان يكون الاسم فارغ ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "لا يجب ان يكزن الايميل فارغ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "لا يجب ان تكون كلمة السر فارغه")]
        [MinLength(6,ErrorMessage = " يجب ان تكون كلمة السر بين  6 الي256 حرف ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لا يجب ان تكون كلمة السر فارغه")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        public string ConfirmPassword { get; set; }
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "يجيب اختيار الدور")]
        public string RoleId { get; set; }
    }
}
