using ElBasset.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO.DTO
{
    public class VideosDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VideoDecription { get; set; }
        public string VideoLink { get; set; }
        public DateTime dateTime { get; set; }
        public string ImageVideoPath { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
