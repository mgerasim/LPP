using LPP.Bot.Core;
using LPP.Bot.Core.Handler;
using LPP.Bot.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LPP.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBot(this IServiceCollection services)
        {
            services.AddHostedService<TelegramBotService>();

            services.AddWindowsService(options =>
            {
                options.ServiceName = "LPP";
            });

            services.AddSingleton<UserStateManager>();

            services.AddScoped<CurrentUserState>();

            services.AddOptions<BotConfiguration>()
                            .BindConfiguration(nameof(BotConfiguration));

            services.AddScoped<StartHandler>();
            services.AddScoped<KeyboardHandler>();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Регистрация всех обработчиков IHandler
            services.AddAssemblyServices(typeof(IHandler).Assembly);
        }

        public static void AddAssemblyServices(this IServiceCollection services, Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var handlerType in handlerTypes)
            {
                // Регистрация с использованием фабрики
                services.AddScoped(typeof(IHandler), handlerType);
            }
        }
    }
}
