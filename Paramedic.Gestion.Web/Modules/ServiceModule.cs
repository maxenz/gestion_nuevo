using Autofac;
using System.Linq;
using System.Reflection;

namespace Paramedic.Gestion.Web.Modules
{

    public class ServiceModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder
                .RegisterAssemblyTypes(Assembly.Load("Paramedic.Gestion.Service"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }

    }
}