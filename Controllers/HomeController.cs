using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shepilov.Nikita._5H.Secondaweb.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Shepilov.Nikita._5H.SecondaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if( User.Identity.IsAuthenticated )
                return View();

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            if( User.Identity.IsAuthenticated )
                return View();

            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
public IActionResult Grazie()
        {
            var  db = new PrenotazioneContext();
            return View();
        }


        [HttpGet]
        public IActionResult Prenota()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Prenota(Prenotazione p)
        {
            //p.Data = DateTime.Now; 

            PrenotazioneContext db = new PrenotazioneContext();     
            db.Prenotazioni.Add(p);
            db.SaveChanges();

            //Prenotazione.Add(p);
            return View("Grazie", db.Prenotazioni);
        }
////////////////////////////////////////////////////////////
          public IActionResult Cancella(int id)
        {
            var db=new PrenotazioneContext();
            Prenotazione prenotazione = db.Prenotazioni.Find(id);
            db.Remove(prenotazione);
            db.SaveChanges();
            return View("Cancella",db);
        }

/////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public IActionResult Modifica(int Id)
        {
            var db = new PrenotazioneContext();
            Prenotazione prenotazione=db.Prenotazioni.Find(Id);
            if(prenotazione != null)
                return View("Modifica",prenotazione);

            return NotFound();
        }

        [HttpPost]
        //public IActionResult Modifica(int id, [Bind("Nome,Email")] Prenotazione nuovo)
        //{
        public IActionResult Modifica(int id,Prenotazione nuovo)
        {
            var db = new PrenotazioneContext();
            var vecchio=db.Prenotazioni.Find(id);
            if(vecchio!=null){
                    vecchio.Nome=nuovo.Nome;
                    vecchio.Email=nuovo.Email;
                db.Prenotazioni.Update(vecchio);
                db.SaveChanges();
                return View("Grazie",db.Prenotazioni);
            }
            return NotFound();
        }
/////////////////////////////////////////////////////////////////////////7
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
            public IActionResult Upload(CreatePost post)
        {
            MemoryStream stream  = new MemoryStream();
            post.MyCSV.CopyTo(stream);
            stream.Seek(0,0);
            StreamReader fin = new StreamReader(stream);
            
            if(!fin.EndOfStream)
            {
                PrenotazioneContext db = new PrenotazioneContext();     
               string riga = fin.ReadLine();
               while(!fin.EndOfStream)
               {
                riga = fin.ReadLine();
                string[] colonne = riga.Split(';');
                Prenotazione p=new Prenotazione{Nome=colonne[0],Email=colonne[1],DataPrenotazione=Convert.ToDateTime(colonne[2])};
                 db.Prenotazioni.Add(p);
               }     
               db.SaveChanges();

            return View("Grazie", db.Prenotazioni);
            }
            return View();
            }
    }
}
