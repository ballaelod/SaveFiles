using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaveFiles.Startup))]
namespace SaveFiles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
