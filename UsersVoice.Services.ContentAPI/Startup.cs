using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using MediatR;
using UsersVoice.Services.ContentAPI.CQRS.Queries;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace UsersVoice.Services.ContentAPI
{
    public class Startup
    {
        private Container _container = new Container();
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc()
               .UseSimpleInjectorAspNetRequestScoping(_container);

            InitContainer(app);
        }

        private void InitContainer(IApplicationBuilder app)
        {
            _container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            var assemblies = GetAssemblies().ToArray();
            _container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            _container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);

            _container.RegisterSingleton(_env);

            RegisterMediator(_container);

            _container.Verify();
        }

        private static void RegisterMediator(Container container)
        {
            container.RegisterSingleton<IMediator, Mediator>();

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(UserAvatarQuery).GetTypeInfo().Assembly;
        }
    }
}
