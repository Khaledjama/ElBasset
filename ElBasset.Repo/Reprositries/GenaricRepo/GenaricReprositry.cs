using ElBasset.DataBase.DataBase;
using ElBasset.Repo.Interfacies.GenaricInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElBasset.Repo.Reprositries.GenaricRepo
{
    public class GenaricReprositry<T> : IGenaricReprositry<T> where T : class
    {
        protected AppDbContext _context;
        public GenaricReprositry(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Insert(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
        //public void Updating(object id, T obj)
        //{
        //    var CurrentValues = GetById(id);
        //    this._context.Entry(CurrentValues).CurrentValues.SetValues(obj);
        //}

        public void Delete(object id)
        {
            var CuurnetObject = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(CuurnetObject);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async void SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


    }

}
