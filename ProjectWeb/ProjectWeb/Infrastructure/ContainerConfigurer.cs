using Autofac;
using Autofac.Extensions.DependencyInjection;
using ProjectWeb.Data;
using ProjectWeb.Interfaces;
using ProjectWeb.Services;

namespace ProjectWeb.Infrastructure
{
    public class ContainerConfigurer
    {
        public static IServiceProvider ConfigureContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Adaugă dependințele aplicației
            builder.Populate(services);


            // Înregistrează serviciile
            builder.RegisterType<ApplicationDB>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ZborService>().As<IZborService>().InstancePerLifetimeScope();
            builder.RegisterType<UtilizatorService>().As<IUtilizatorService>().InstancePerLifetimeScope();  // Înregistrarea serviciului UtilizatorService
            builder.RegisterType<CheckinService>().As<ICheckinService>().InstancePerLifetimeScope();


            // Construiește containerul
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        internal static void ConfigureContainer(object v)
        {
            throw new NotImplementedException();
        }
    }
}
