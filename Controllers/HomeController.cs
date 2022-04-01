using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    public class HomeController : Controller
    {
        public Entities FitnesManager = new Entities();

        public ActionResult Index()
        {
            List<Usluga> uslugas =
                FitnesManager.Uslugas.ToList();
            FitnessViewModel views = new FitnessViewModel();

            foreach (var usluga in uslugas)
            {
                UslugaViewModel view = new UslugaViewModel();
                view.Nazvanie = usluga.Nazvanie;
                view.Cena_za_poseshenie =
                     usluga.Cena_za_poseshenie;
                view.ImagePath = @"Pictures\" + usluga.Nazvanie + ".jpg";//File(usluga.Image, "");
                views.Uslugas.Add(view);
            }

            return View(views);
        }
    }
}