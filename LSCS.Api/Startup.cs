using System;
using System.Net.Http.Formatting;
using System.Web.Configuration;
using System.Web.Http;
using LSCS.Api.Filters;
using LSCS.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace LSCS.Api
{
    public class Startup
    {

        public string RepositoryConnectionString { get; private set; }
        public string DatabaseName { get; private set; }
        public int MaximumPageSize { get; private set; }

        public Startup()
        {
            ReadConfigurationParameters();
        }

        private void ReadConfigurationParameters()
        {
            RepositoryConnectionString = WebConfigurationManager.ConnectionStrings["LSCS.Database"].ToString();
            DatabaseName = WebConfigurationManager.AppSettings["databaseName"];
            MaximumPageSize = Convert.ToInt32(WebConfigurationManager.AppSettings["apiMaxPageSize"]);
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureJson(config);
            InjectDependencies(config);

            // Register filters
            config.Filters.Add(new ExceptionHandlerFilter());
            config.Filters.Add(new LoggingFilter());

            // Map API routes
            config.MapHttpAttributeRoutes();

            // Register configuration
            app.UseWebApi(config);
        }

        private void ConfigureJson(HttpConfiguration config)
        {
            config.Formatters.Clear();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.Add(new JsonMediaTypeFormatter { SerializerSettings = serializerSettings });
        }

        private void InjectDependencies(HttpConfiguration config)
        {
            var container = new Container();

            container.RegisterSingle<IChecklistRepository>(() => new ChecklistRepository(RepositoryConnectionString, DatabaseName));
            container.RegisterSingle<IControllerConfiguration, ControllerConfiguration>();

            container.RegisterWebApiControllers(config);
            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
