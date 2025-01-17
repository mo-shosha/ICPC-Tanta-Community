using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        public BaseRepository(AppDbContext db)
        {
            _db = db;   
        }
        public T Add(T entity)
        {
            _db.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _db.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _db.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public int Count()
        {
            return _db.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _db.Set<T>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _db.Set<T>().CountAsync(criteria);
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public T GetById(int id)
        {
            return _db.Set<T>().Find(id);
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public T Update(T entity)
        {
            _db.Set<T>().Update(entity);
            return entity;
        }

        
    }
}
