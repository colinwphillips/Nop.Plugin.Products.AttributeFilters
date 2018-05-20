using System;
using Nop.Core.Plugins;
using Nop.Services.Common;

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
            Console.WriteLine($"Installed {nameof(AttributeFilterPlugin)}.");
            base.Install();
        }

        public override void Uninstall()
        {
            Console.WriteLine($"Uninstalled {nameof(AttributeFilterPlugin)}.");
            base.Uninstall();
        }
    }
}
