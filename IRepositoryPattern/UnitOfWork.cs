using SalesInvoice.Data;
using SalesInvoice.Models;

namespace SalesInvoice.IRepositoryPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            order = new Repository<Order>(context);
            product = new Repository<Product>(context);
            category = new Repository<Category>(context);
            orderItems = new Repository<OrderItems>(context);
        }
        public IRepository<Order> order { get; private set; }

        public IRepository<Product> product { get; private set; }

        public IRepository<Category> category { get; private set; }

        public IRepository<OrderItems> orderItems { get; private set; }

        public void Dispose()
        {
            context.Dispose();
        }
        public async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
