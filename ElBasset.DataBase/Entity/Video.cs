﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DataBase
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VideoDecription { get; set; }
        public string VideoLink { get; set; }
        public DateTime dateTime { get; set; }
        public string ImageVideoPath { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public Video()
        {
            this.dateTime = DateTime.Now;
        }
    }
}
