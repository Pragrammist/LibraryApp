using System.Web.Mvc;
using LibraryApp.DataAccess;
using LibraryApp.DataAccess.Repositories;
using LibraryApp.Services.BookClient;
using Unity;
using Unity.Mvc5;

namespace LibraryApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            
            container.RegisterType<IConnectionFactory, ConnectionFactory>();
            container.RegisterType<IClientBookUnitOfWork, ClientBookUnitOfWork>();
            
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IBookService, BookService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}