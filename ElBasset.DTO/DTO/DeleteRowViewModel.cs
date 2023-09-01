using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class DeleteRowViewModel
    {
        [Required]
        public string RoleId { get; set; }
    }
}
