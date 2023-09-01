using ElBasset.DataBase;
using ElBasset.DataBase.DataBase;
using ElBasset.Repo.Interfacies.GenaricInterface;
using ElBasset.Repo.Reprositries.GenaricRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;
        public IGenaricReprositry<Exam> ExamRepo { get; private set; }

        public IGenaricReprositry<ExamType> ExamTypeRepo { get; private set; }

        public IGenaricReprositry<Department> DepartmentRepo { get; private set; }

        public IGenaricReprositry<Video> VideoRepo { get; private set; }

        public IGenaricReprositry<Lecture> LecturetRepo { get; private set; }

        public int Complete()
        {
            return _Context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
        public UnitOfWork(AppDbContext Context)
        {
            _Context = Context;
            ExamRepo = new GenaricReprositry<Exam>(_Context);
            ExamTypeRepo = new GenaricReprositry<ExamType>(_Context);
            DepartmentRepo = new GenaricReprositry<Department>(_Context);
            VideoRepo = new GenaricReprositry<Video>(_Context);
            LecturetRepo = new GenaricReprositry<Lecture>(_Context);

        }


    }

}
