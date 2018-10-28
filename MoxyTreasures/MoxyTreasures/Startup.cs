using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoxyTreasures.Startup))]
namespace MoxyTreasures
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
