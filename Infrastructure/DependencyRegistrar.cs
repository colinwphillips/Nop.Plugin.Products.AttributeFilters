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
using Nop.Core.Plugins;
using Nop.Services.Catalog;
using Nop.Web.Factories;

namespace Nop.Plugin.Products.AttributeFilters.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            var installed = IsPluginInstalled(typeFinder);
            if (!installed) return;
            builder.RegisterType<ProductServiceCustom>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductModelFactoryCustom>().As<IProductModelFactory>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerLifetimeScope();
        }

        private static bool IsPluginInstalled(ITypeFinder typeFinder)
        {
            var isInstalled = false;
            var types = typeFinder.FindClassesOfType<IPluginFinder>(true).ToList();
            if (types.Count() != 1) return false;
            var plugins = Activator.CreateInstance(types.First()) as IPluginFinder;
            var thisPlugin = GetPluginInfo();
            if (thisPlugin == null) return false;
            var plug = plugins?.GetPluginDescriptorBySystemName(thisPlugin.SystemName);
            if (plug != null && plug.Installed)
                isInstalled = true;

            return isInstalled;
        }

        private static PluginDescriptor GetPluginInfo()
        {
            try
            {
                var pluginFullName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                var pluginShortName = pluginFullName.Replace("Nop.Plugin.", string.Empty);
                var json = File.ReadAllText($"Plugins/{pluginShortName}/plugin.json");
                var descr = JsonConvert.DeserializeObject<PluginDescriptor>(json);
                return descr;
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    Console.WriteLine(e);
                throw;
            }
        }

        public int Order => 2;
    }
}
