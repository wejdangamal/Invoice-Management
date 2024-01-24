using SalesInvoice.Models;

namespace SalesInvoice.IRepositoryPattern
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Order> order { get; }
        IRepository<Product> product { get; }
        IRepository<Category> category { get; }
        IRepository<OrderItems> orderItems { get; }
        Task<bool> SaveChanges();
    }
}
