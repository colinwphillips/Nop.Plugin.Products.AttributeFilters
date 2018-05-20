using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Services.Catalog;
using Nop.Web.Factories;

namespace Nop.Plugin.Products.AttributeFilters.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {

            builder.RegisterType<ProductServiceCustom>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductModelFactoryCustom>().As<IProductModelFactory>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();
        }

        public int Order => 2;
    }
}
