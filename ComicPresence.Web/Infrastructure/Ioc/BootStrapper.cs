using System.Web.Mvc;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

using ComicPresence.Data.Interfaces;
using ComicPresence.Data.Repo;
using ComicPresence.Web.Infrastructure.Containers;

namespace ComicPresence.Web.Infrastructure.Ioc
{
    public class BootStrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // Add dependencies here.
            container.RegisterType<IComicRepo, ComicRepo>();
            CPUnityContainer.Container = container;

            return container;
        }
    }
}