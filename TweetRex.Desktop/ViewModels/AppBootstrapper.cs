using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Builder;
using ReactiveUI;
using ReactiveUI.Routing;

namespace TweetRex.Desktop.ViewModels
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper():this(new RoutingState())
        {            
        }

        public AppBootstrapper(IRoutingState router)
        {
            if (router == null) throw new ArgumentNullException("router");
            Router = router ;
            var container = CreateContainer();
            RxApp.ConfigureServiceLocator(
                (serviceType, contract) => {
                        if (contract != null) return container.ResolveNamed(contract, serviceType);
                        return container.Resolve(serviceType);
                    },
                (serviceType, contract) => {
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
                    var instance = contract != null
                        ? (IEnumerable)container.ResolveNamed(contract, enumerableType)
                        : (IEnumerable)container.Resolve(enumerableType);
                    return instance.Cast<object>();
                }, 
                (impl, serviveType, contract) => {
                    var builder = new ContainerBuilder();
                    builder.RegisterType(impl).As(serviveType);
                    if (contract != null)
                    {
                        builder.RegisterType(impl).Named(contract, serviveType);
                    }
                    builder.Update(container);
                }
            );

            // TODO: This is a good place to set up any other app 
            // startup tasks, like setting the logging level
            LogHost.Default.Level = LogLevel.Debug;


            // Navigate to the opening page of the application
            var vm = RxApp.GetService<IHomeViewModel>();
            Router.Navigate.Execute(vm);

        }

        public IRoutingState Router { get; private set; }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IScreen>().SingleInstance();
            builder.RegisterType<RoutingState>().As<IRoutingState>();
            builder.RegisterAssemblyTypes(typeof (AppBootstrapper).Assembly)
                .Where(t => t != typeof(AppBootstrapper))
                .AsSelf().AsImplementedInterfaces();

            var container = builder.Build();
            return container;
        }
    }
}
