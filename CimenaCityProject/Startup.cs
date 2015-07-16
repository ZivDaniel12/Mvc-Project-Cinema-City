using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CimenaCityProject.Startup))]
namespace CimenaCityProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
