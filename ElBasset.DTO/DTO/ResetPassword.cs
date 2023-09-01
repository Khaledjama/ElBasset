using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "لا يجب ان تكون كلمة السر فارغه")]
        [StringLength(255, ErrorMessage = "يجب ان تكون كلمة السر بين 6الي 255 حرف", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لا يجب ان تكون كلمة السر فارغه")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
