using ElBasset.DataBase;
using ElBasset.Repo.Interfacies.GenaricInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenaricReprositry<Exam> ExamRepo { get; }
        IGenaricReprositry<ExamType> ExamTypeRepo { get; }
        IGenaricReprositry<Department> DepartmentRepo { get; }
        IGenaricReprositry<Video> VideoRepo { get; }
        IGenaricReprositry<Lecture> LecturetRepo { get; }
        //ITestReprositry TestRepo { get; }

        int Complete();
        Task<int> CompleteAsync();
    }

}
