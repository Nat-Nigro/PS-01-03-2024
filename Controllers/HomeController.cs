using PS_01_03_2024.Models;
using System.Web.Mvc;

namespace PS_01_03_2024.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // Action per la visualizzazione dei dati dalla tabella Angrafica
        public ActionResult ListaTrasgressori()
        {
            return View(Trasgressore.VisualizzaTragressori());
        }

        // Richiamo la View per inserire un nuovo trasgressore
        public ActionResult NewTrasgressore()
        {
            return View();
        }
        // Richiamo il metodo CreaTrasgressore del modello Trasgressore

        [HttpPost]
        public ActionResult NewTrasgressore(Trasgressore trasgressore)
        {
            Trasgressore.CreaTrasgressore(trasgressore);
            return RedirectToAction("ListaTrasgressori");
        }

        //Richiamo le violazioni
        public ActionResult ListaViolazioni()
        {
            return View(Violazione.GetViolazioni());
        }

        // Action per la visualizzazione dei dati dalla tabella Verbali
        public ActionResult ListaVerbali()
        {
            return View(Verbale.GetVerbale());
        }


        public ActionResult NewVerbale()
        {
            return View();
        }
        //Action per la creazione di un verbale
        [HttpPost]
        public ActionResult NewVerbale(Verbale verbale)
        {
            Verbale.CreaVerbale(verbale);
            return RedirectToAction("ListaVerbali");
        }

    }
}

