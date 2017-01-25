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
      
        static void Main(string[] args)
        {
            
            var grabby = new Grabby();
            List<Adress> Guides ;
        
            ObjectId id;
            String output;
    
            var c = new AdressRepository();
        
            Guides = c.AllAdresses()
                .Select(r => new Adress()
                {
                    Id=r.Id,
                    idu=r.idu,
                    Adres = r.Adres,
                    Description = r.Description,
                    

                }).ToList();

           
                id = Guides[Guides.Count()-1].Id;
                output = grabby.Grab(Guides[Guides.Count() - 1].Adres);
                Adress adr = c.GetById(id);
                adr.GrabbedWeb = output;
                c.Update(adr);


        
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
           
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output;
            }
        }
    
}
