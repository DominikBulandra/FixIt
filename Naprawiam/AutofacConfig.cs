using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Naprawiam.Repositories;
using Naprawiam.Controllers;
using System.Web.Mvc;
namespace Naprawiam
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<GuideRepository>().As<IRepository<Guide>>().SingleInstance();
            //builder.RegisterType<AdressRespository>().As<AdressRespository>().SingleInstance();


            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


        }
    }
}