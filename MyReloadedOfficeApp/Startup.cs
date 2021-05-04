using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyReloadedOfficeApp.Startup))]
namespace MyReloadedOfficeApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
