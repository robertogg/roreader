using System;
using System.IdentityModel.Services;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using RoReader.AzureStorage;
using RoReader.AzureStorage.Core;
using RoReader.AzureStorageTable;
using RoReader.Config;
using RoReader.Infrastructure.Cache;
using RoReader.Infrastructure.Claims;
using RoReader.Infrastructure.Core;

namespace RoReader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IdentityConfig.ConfigureIdentity();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(CreateStandardKernel()));
        }

        void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            if (!String.IsNullOrEmpty(IdentityConfig.Realm))
            {
                e.SignInRequestMessage.Realm = IdentityConfig.Realm;
            }
        }

        private IKernel CreateStandardKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind(typeof(IRepository<>))
                .To(typeof(Repository<>));

            kernel.Bind(typeof(IClaimsInfo))
               .To(typeof(ClaimsInfo));

            kernel.Bind(typeof(IAzureQueue))
              .To(typeof(AzureQueue));

            kernel.Bind(typeof(IAzureCache))
            .To(typeof(AzureCache));

            return kernel;
        }
    }
}
