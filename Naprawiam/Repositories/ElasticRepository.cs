using MongoDB.Driver.Core.Configuration;
using Naprawiam.ViewModel;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Naprawiam.Repositories
{
    public static class ElasticRepository
    {
        public static void ReindexFromWebGrabber()
        {
            var connectionSettings = new Nest.ConnectionSettings(new Uri("http://localhost:9200/"));

            var elasticClient = new ElasticClient(connectionSettings);
            var node = new Uri("http://myserver:9200");
            var settings = new Nest.ConnectionSettings(node);
            var client = new ElasticClient(settings);
            elasticClient.DeleteIndex("adressesindex");
            elasticClient.CreateIndex("adressesindex");
            //var setting = new Nest.ConnectionSettings("localhost", 9200);
            //var client = new Nest.ElasticClient(setting);
            var c = new AdressRepository();
            ElasticResult viewModel = new ElasticResult
            {

                Guides = c.AllAdresses()
                 .Select(r => new Models.elastic()
                 {
                     idu = r.idu,
                     Adres = r.Adres,
                     Description = r.Description,
                     GrabbedWeb = r.GrabbedWeb

                 }).ToList()

            };
            foreach (var dep in viewModel.Guides)
            {
                var response = elasticClient.Index(dep, idx => idx.Index("adressesindex") /*"fix", "adresses", dep.Id*/);
            }

        }

    }
}