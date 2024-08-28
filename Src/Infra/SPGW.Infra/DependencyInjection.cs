using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SPGW.Domain;
using SPGW.Domain.Common;
using SPGW.Domain.Customer.Repositories;
using SPGW.Domain.Order.Entities;
using SPGW.Domain.Order.Repositories;
using SPGW.Domain.Psp.Repositories;
using SPGW.Infra.Common;
using SPGW.Infra.Contexts;
using SPGW.Infra.Helpers;
using SPGW.Infra.Models.Customer.Repositories;
using SPGW.Infra.Models.Order.Repositories;
using SPGW.Infra.Models.Psp.Repositories;

namespace SPGW.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDomain();

            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddDbContext<SPGWContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LOCAL_DB"))
               .UseLazyLoadingProxies()); // If you need lazy loading proxies

            //services.AddTransient<IBPMService, BPMService>();
            //services.AddTransient<IPNAService, PNAService>();
            services.AddTransient<IIRKService, IRKService>();

            services.AddTransient<IPEPService, PEPService>();

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IGWService, GWService>();

            services.AddTransient<IUserManagerService, UserManagerService>();
            services.AddTransient<IGWService, GWService>();
                             
            services.AddTransient<IEFUnitOfWorkFactory, FRMSEFUnitOfWorkFactory>();
            services.AddTransient<IEFUnitOfWorkScope, EfUnitOfWorkScope>();
            services.AddTransient<IEFUnitOfWork, EFUnitOfWork>();
            services.AddTransient<PerHttpEFContextUnitOfWorkScope>();

            services.AddTransient(typeof(IEFRepository<>), typeof(EFRepository<>));
                        


            services.AddOptions();
            services.Configure<InfraSetting>(t => configuration.GetSection("InfraSetting"));
            
            return services;
        }
    }
}
