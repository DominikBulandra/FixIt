using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Naprawiam.Repositories;
using Naprawiam.ViewModel;
using Naprawiam.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nest;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.IO;


//using Nest;

namespace Naprawiam.Controllers
{
    public class FixController : Controller
    {


        Repositories.IRepository<Guide> _GuideRepository;
        //IAdressRespository _MongoRepository;
        public FixController(Repositories.IRepository<Guide> GuideRepository/*, IAdressRespository MongoRepository*/)

        {
            _GuideRepository = GuideRepository;
           // _MongoRepository = MongoRepository;
        }

        // GET: Fix
        
        public ActionResult Index()
        {
            return View();
        }

        //Show all instructions
        public ActionResult List()
        {
            //create variable c with repository of Mongo Database
            var c = new AdressRepository();

            FixListViewModel viewModel = new FixListViewModel
            {
                // Viewmodel include Guides list of FixCreateViewModels objects, function AllAdresses returns all adreses from database 
                Guides = c.AllAdresses()
                .Select(r => new FixCreateViewModel()
                {
                    Adres = r.Adres,
                    Description = r.Description,
                }).ToList()

            };
            return View(viewModel);
        }
        //Show only instruction from actual logged user 
        public ActionResult ListOf(String id)
        {
            //id is email adres f user
            //create variable c with repository of Mongo Database

            var c = new AdressRepository();
            FixListViewModel viewModel = new FixListViewModel
            {

                // Viewmodel include Guides list of FixCreateViewModels objects, function GetByUSerId return adresses from specified user
                Guides = c.GetByUserId(id)
             .Select(r => new FixCreateViewModel()
             {
                 Adres = r.Adres,
                 Description = r.Description,
                 idu = r.idu,
                 StringObjectId = r.Id.ToString()
             }).ToList()

            };
            return View(viewModel);
        }
        //Delete Adres from database 
        public ActionResult DeleteAdres(string id)
        {
            //ObjectId ID = new ObjectId();
            //create variable c with repository of Mongo Database
            var c = new AdressRepository();
            //Convert string id to ObjectId id
            var oId = ObjectId.Parse(id);
            // function Rewmove delete specified adres
            bool t=c.Remove(oId);
            return RedirectToAction("ReIndex");

        }


        // Main Task of this actionresult is Index all documents from mongodb to elastic search engine
        public ActionResult ReIndex()
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));

            var elasticClient = new ElasticClient(connectionSettings);
            var node = new Uri("http://myserver:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            //delete old indexes and create new
            elasticClient.DeleteIndex("adressesindex");
            elasticClient.CreateIndex("adressesindex");
            
            var c = new AdressRepository();
            ElasticResult viewModel = new ElasticResult
            {

                Guides = c.AllAdresses()
                 .Select(r => new Models.elastic()
                 {
                     idu=r.idu,
                     Adres = r.Adres,
                     Description = r.Description,
                     GrabbedWeb=r.GrabbedWeb
                     
                 }).ToList()

            };
            foreach (var dep in viewModel.Guides)
            {
                //insert new index foreach element in Guides list
                var response = elasticClient.Index(dep, idx => idx.Index("adressesindex") /*"fix", "adresses", dep.Id*/);
            }
        
            return RedirectToAction("Index");
        }

        //search in indexed document using elastic search
        public ActionResult Search(string q)
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/"));

            var elasticClient = new ElasticClient(connectionSettings);
            // create result from elasticsearchengine using query with parameter q 
            var result = ElasticClient.Search<elastic>(body =>
           body.Query(query =>
           query.QueryString(qs => qs.Query(q))));
         
           var genre = new Genre()
            {
                Name = "Search results for " + q,

                Adresses= result.Documents.ToList()
            };
            FixListViewModel viewModel = new FixListViewModel
            {

                Guides = genre.Adresses
              .Select(r => new FixCreateViewModel()
              {
                  Adres = r.Adres,
                  Description = r.Description,
              }).ToList()

            };
            return View(viewModel);
           
        }
        private static ElasticClient ElasticClient
        {
            get
            {

                var node = new Uri("http://localhost:9200");
                var setting = new ConnectionSettings(node);
               
                setting.DefaultIndex("adressesindex");
                //var client = new ElasticClient(setting);
                return new ElasticClient(setting);
            }
        }





        static List<Adress> Departments=new List<Adress>();
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MassageAdres="";
            FixCreateViewModel viewModel = new FixCreateViewModel();
            


            return View(viewModel);
        }
        //create new adres in database
        [HttpPost]
        public ActionResult Create(FixCreateViewModel viewModel)
        {

            if (ModelState.IsValid && ValidateAdres(viewModel.Adres))
            {


                var c = new AdressRepository();
                Adress newadress = new Adress();
                newadress.idu = User.Identity.Name;
                newadress.Adres = viewModel.Adres;
                newadress.Description = viewModel.Description;
                c.Add(newadress);
                //process qith web grabber
              
                Process pcx = new System.Diagnostics.Process();
                ProcessStartInfo pcix = new System.Diagnostics.ProcessStartInfo();
                pcix.FileName = @"C:\Users\Dominik\Documents\visual studio 2015\Projects\Naprawiam\PhantomWebGrab\bin\Debug\\PhantomWebGrab.exe";
                //pcix.Arguments = WrkGalId.ToString() + " " + websiteId.ToString() + "" + " " + "19" + " \"" + dFileName + "\" ";
                pcix.UseShellExecute = true;
                pcix.WindowStyle = ProcessWindowStyle.Hidden;
                pcx.StartInfo = pcix;
                pcx.Start();
                pcx.WaitForExit();
                
                return RedirectToAction("ReIndex");
            }
            if (!ValidateAdres(viewModel.Adres))
            {
                viewModel.Adres = "błąd";
                ViewBag.MassageAdres = "niepoprawny adres:(";
            }
            return View(viewModel);
        }
        public static bool ValidateAdres(string adres)
        {
          
            bool IsResponsed = false;
            WebResponse response;
            try
            {
                WebRequest request = WebRequest.Create(
           adres);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                response = request.GetResponse();
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                IsResponsed = true;
                response.Close();
            }
            catch (Exception)
            {
                IsResponsed = false;
               // wrong url adres
            }
            return IsResponsed;
        }
    }
    public class Guide : IIdentity
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public int UserId { get; set; }

        public string Desc { get; set; }
    }
   
}