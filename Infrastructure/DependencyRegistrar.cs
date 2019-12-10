using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core;
using Newtonsoft.Json;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Services.Plugins;
using Nop.Services.Catalog;
using Nop.Services.Events;
using Nop.Services.Plugins;
using Nop.Web.Factories;

namespace Nop.Plugin.Products.AttributeFilters.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //var installed = IsPluginInstalled();
            //if (!installed) return;

            builder.RegisterType<ProductServiceCustom>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductModelFactoryCustom>().As<IProductModelFactory>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();
        }

        //register after avalara as they also override shoppingcartmodelfactory
        public int Order => 5;
    }
}
