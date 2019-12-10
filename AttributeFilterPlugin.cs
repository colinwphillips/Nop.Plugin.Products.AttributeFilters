using System;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Products.AttributeFilters
{
    public class AttributeFilterPlugin : BasePlugin, IMiscPlugin
    {
        public override PluginDescriptor PluginDescriptor { get => base.PluginDescriptor; set => base.PluginDescriptor = value; }

        public override string GetConfigurationPageUrl()
        {
            return base.GetConfigurationPageUrl();
        }

        public override void Install()
        {
            Console.WriteLine($"Installed {this.GetType().Name}.");
            base.Install();
        }

        public override void Uninstall()
        {
            Console.WriteLine($"Uninstalled {this.GetType().Name}.");
            base.Uninstall();
        }
    }
}
