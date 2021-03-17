using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Repositories
{

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        //protected readonly ApplicationDbContext _context;

        //public Repository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<T> Findsaas(Expression<Func<T, bool>> expression)
        //{
        //    return await _context.Set<T>().FirstOrDefaultAsync(expression);
        //}

        //public void Add(T entity)
        //{
        //    _context.Set<T>().Add(entity);
        //}
        //public void AddRange(IEnumerable<T> entities)
        //{
        //    _context.Set<T>().AddRange(entities);
        //}
        //public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        //{
        //    return _context.Set<T>().Where(expression);
        //}
        //public IEnumerable<T> GetAll()
        //{
        //    return _context.Set<T>().ToList();
        //}
        //public T GetById(int id)
        //{
        //    return _context.Set<T>().Find(id);
        //}
        //public void Remove(T entity)
        //{
        //    _context.Set<T>().Remove(entity);
        //}
        //public void RemoveRange(IEnumerable<T> entities)
        //{
        //    _context.Set<T>().RemoveRange(entities);
        //}

        //public IQueryable<T> Include<T>(Expression<Func<T, Object>> criteria) where T : class
        //{
        //    return _context.Set<T>().Include(criteria);
        //}
    }
}
