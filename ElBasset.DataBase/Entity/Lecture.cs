using ElBasset.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DataBase
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PdfLink { get; set; }
        public string? VideoSrc { get; set; }
        public DateTime dateTime { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public Lecture()
        {
            this.dateTime = DateTime.Now;
        }
    }
}
