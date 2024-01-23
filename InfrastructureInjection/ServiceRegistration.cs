using SalesInvoice.IRepositoryPattern;

namespace SalesInvoice.InfrastructureInjection
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
