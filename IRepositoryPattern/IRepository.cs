namespace SalesInvoice.IRepositoryPattern
{
    public interface IRepository<T>:IDisposable
    {
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int Id);
        Task<T> GetById(int id);
        IList<T> GetAll(int page,int size);
        IList<T> GetAll(string[] includes =null);
    }
}
