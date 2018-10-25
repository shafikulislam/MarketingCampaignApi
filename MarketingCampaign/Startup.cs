using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarketingCampaign.Startup))]
namespace MarketingCampaign
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
