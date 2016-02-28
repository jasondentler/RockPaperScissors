using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RockPaperScissors.Web.Startup))]
namespace RockPaperScissors.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
