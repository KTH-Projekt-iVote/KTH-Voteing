using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iVoteMVC.Startup))]
namespace iVoteMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
