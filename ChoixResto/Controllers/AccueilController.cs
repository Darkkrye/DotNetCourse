using ChoixResto.Models;
using ChoixResto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChoixResto.Controllers
{
    public class AccueilController : Controller
    {
        // GET: Accueil
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult AfficheListeRestaurant()
        {
            List<Resto> listeDesRestos = new List<Resto>
            {
                new Resto { Id = 1, Name = "Resto pinambour", Phone = "1234" },
                new Resto { Id = 2, Name = "Resto tologie", Phone = "1234" },
                new Resto { Id = 5, Name = "Resto ride", Phone = "5678" },
                new Resto { Id = 9, Name = "Resto toro", Phone = "555" }
            };

            return PartialView(listeDesRestos);
        }
    }
}