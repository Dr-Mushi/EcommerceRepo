using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingEFRelations.Data;
using TestingEFRelations.Models;
using TestingEFRelations.Repositories.Interface;

namespace TestingEFRelations.Repositories
{
    public class Repository<T> : IRepository<T>
    {

        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public double TotalAsync(double entityTotal ,IEnumerable<T> entitySize)
        {
            double total = 0;

            //sum of all cart tables totals
            foreach (var count in entitySize)
            {
                total += entityTotal;
            }

            return total;
        }

        public Task<bool> DeleteSameItem(int? id)
        {
            throw new NotImplementedException();
        }



        public Task<bool> HasSameItem(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
