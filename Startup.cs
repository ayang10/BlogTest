using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogTest.Startup))]
namespace BlogTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
