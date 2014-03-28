using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure;
using Owin;
using RoReader.Constants;

namespace RoReader
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseRedis(CloudConfigurationManager.GetSetting(Azure.RedisServer), 6379, CloudConfigurationManager.GetSetting(Azure.RedisPassword), "roreader");
            app.MapSignalR();
        }
    }
}