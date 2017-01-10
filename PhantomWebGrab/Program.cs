using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Naprawiam.Repositories;
using Naprawiam.Models;
using Naprawiam.Controllers;
using MongoDB.Bson;
using System.Collections;
using Nest;
using Naprawiam.ViewModel;

namespace PhantomWebGrab
{
    class Program
    {
        //List<Adress> GuidesList = new List<Adress>() ;
        static void Main(string[] args)
        {
            
            var grabby = new Grabby();
            List<Adress> Guides ;
            // output = grabby.Grab("http://forum.mobileos.pl/watek-Instrukcja-Instrukcja-naprawy-zbitej-szyby-przy-uzyciu-kleju-LOCA_5571");
            //Console.WriteLine(output);
            ObjectId id;
            String output;
            //= new ObjectId("573dbafbfd6367066050cdce");
            var c = new AdressRepository();
            //Guides = c.AllAdresses().ToList();
            Guides = c.AllAdresses()
                .Select(r => new Adress()
                {
                    Id=r.Id,
                    idu=r.idu,
                    Adres = r.Adres,
                    Description = r.Description,
                    

                }).ToList();

            //Adress adr = c.GetById(id);
            //adr.GrabbedWeb =output ;
            //PONIZSZE : ZBIERANIE WSZYSTKICH STRON!
            //for (int i = 0; i < Guides.Count(); i++)
            //{
            //    id=Guides[i].Id;
            //    output = grabby.Grab(Guides[i].Adres);
            //    Adress adr = c.GetById(id);
            //    adr.GrabbedWeb = output;
            //    c.Update(adr);
            //}
           
                id = Guides[Guides.Count()-1].Id;
                output = grabby.Grab(Guides[Guides.Count() - 1].Adres);
                Adress adr = c.GetById(id);
                adr.GrabbedWeb = output;
                c.Update(adr);


            //c.Update(adr);
            
            //var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));

            //var elasticClient = new ElasticClient(connectionSettings);
            //var node = new Uri("http://myserver:9200");
            //var settings = new ConnectionSettings(node);
            //var client = new ElasticClient(settings);
            //elasticClient.DeleteIndex("adressesindex");
            //elasticClient.CreateIndex("adressesindex");
            ////var setting = new Nest.ConnectionSettings("localhost", 9200);
            ////var client = new Nest.ElasticClient(setting);
            //var d = new AdressRepository();
            //ElasticResult viewModel = new ElasticResult
            //{

            //    Guides = d.AllAdresses()
            //     .Select(r => new Naprawiam.Models.elastic()
            //     {
            //         idu = r.idu,
            //         Adres = r.Adres,
            //         Description = r.Description,
            //         GrabbedWeb = r.GrabbedWeb

            //     }).ToList()

            //};
            //foreach (var dep in viewModel.Guides)
            //{
            //    var response = elasticClient.Index(dep, idx => idx.Index("adressesindex") /*"fix", "adresses", dep.Id*/);
            //}

            Console.WriteLine("koniec");
            
            //Console.ReadLine();

            //File.WriteAllText(@"c:\phantom\test2.txt", output);
        }


        }

        public class Grabby
        {
            public string Grab(string url)
            {
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = @"C:\Users\Dominik\Documents\visual studio 2015\Projects\phantom\phantom\bin\Debug\phantomjs.exe",
                    Arguments = string.Format("\"{0}\\{1}\" {2}", @"C:\Users\Dominik\Documents\visual studio 2015\Projects\Naprawiam\PhantomWebGrab\", "index.js", url)
                };
            //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output;
            }
        }
    
}
