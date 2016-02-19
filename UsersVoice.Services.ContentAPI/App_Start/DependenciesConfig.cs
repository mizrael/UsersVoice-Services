using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using MediatR;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.WebApi;
using UsersVoice.Services.ContentAPI.CQRS.Queries;

namespace UsersVoice.Services.ContentAPI
{
    public class DependenciesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            container.RegisterWebApiControllers(config);

            container.RegisterWebApiRequest<HttpContextBase>(() =>
            {
                var context = HttpContext.Current;
                if (context == null && container.IsVerifying())
                    return null;
                
                return new HttpContextWrapper(context);
            });

            var assemblies = GetAssemblies().ToArray();
            container.Register(typeof (IAsyncRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof (IAsyncNotificationHandler<>), assemblies);

            RegisterMediator(container);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
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