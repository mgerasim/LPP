using LPP.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LPP.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDAL(this IServiceCollection services)
        {
            services.AddDbContext<LPPContext>(
                options =>
                {
                    options.UseSqlite("Data Source=..\\LPP.db");

                    options.EnableSensitiveDataLogging(true);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
    ServiceLifetime.Transient
            );
        }
    }
}
