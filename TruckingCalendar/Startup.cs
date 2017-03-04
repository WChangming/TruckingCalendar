using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TruckingCalendar.Startup))]
namespace TruckingCalendar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
