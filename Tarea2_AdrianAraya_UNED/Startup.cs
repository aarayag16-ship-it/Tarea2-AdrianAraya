using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tarea2_AdrianAraya_UNED.Startup))]
namespace Tarea2_AdrianAraya_UNED
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
