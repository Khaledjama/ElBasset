using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DataBase
{
    public class ApplicationUser : IdentityUser
    {
        public string? Comment { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Adress { get; set; }
        public string? ImagePath { get; set; }
        //--------------------- Relation ------------
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
