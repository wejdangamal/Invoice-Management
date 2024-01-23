using Microsoft.EntityFrameworkCore;
using SalesInvoice.Data;

namespace SalesInvoice.IRepositoryPattern
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }
        public async Task<bool> Add(T entity)
        {
            if (entity is not null)
                entities.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Delete(int Id)
        {
            var found = await this.GetById(Id);
            if(found != null)
                entities.Remove(found);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public IList<T> GetAll(int page=1, int size=10)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;
            var res = entities.Skip((page-1)*size)
                .Take(size)
                .ToList();
            return res;
        }

        public async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IList<T> GetAll(string[] includes = null)
        {
            IQueryable<T> query = entities;
            if(includes != null)
            {
                foreach(var item in includes)
                {
                    query = query.Include(item);
                }
                
            }
          return query.ToList();
        }
    }
}
