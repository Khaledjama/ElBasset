using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video, VideosDTO>();
            CreateMap<VideosDTO, Video>();

            CreateMap<Exam, ExamDTO>();
            CreateMap<ExamDTO, Exam>();

            CreateMap<ExamType, ExamTypeDTO>();
            CreateMap<ExamTypeDTO, ExamType>();

            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>();
            CreateMap<Lecture, LectureDTO>();
            CreateMap<LectureDTO, Lecture>();

            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();

        }
    }
}
