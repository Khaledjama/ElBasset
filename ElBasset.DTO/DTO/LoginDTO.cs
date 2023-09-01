using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "لا يجب ان يكزن الايميل فارغ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "لا يجب ان تكون كلمة السر فارغه")]
        [StringLength(255, ErrorMessage = "يجب ان تكون كلمة السر بين 6الي 255 حرف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
