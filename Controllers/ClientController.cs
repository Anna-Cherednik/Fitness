using Fitness.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    [Authorize(Roles = "user")]
    public class ClientController : Controller
    {
        private ApplicationUserManager _userManager;
        private Entities _fitnesManager;

        public ClientController()
        {
        }

        public ClientController(ApplicationUserManager userManager, Entities fitnesManager)
        {
            UserManager = userManager;
            FitnesManager = fitnesManager;
        }

        public Entities FitnesManager
        {
            get
            {
                return _fitnesManager ?? (_fitnesManager = new Entities());
            }
            private set
            {
                _fitnesManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Client
        public ActionResult Index()
        {
            ApplicationUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
            Klient klient = FitnesManager.Klients.Where(t => t.ID_Login == user.Id).FirstOrDefault();
            List<Abonement> abonements = new List<Abonement>();
            abonements = FitnesManager.Abonements.Where(c => c.ID_klient == klient.ID_klient).ToList();

            List<ClientViewAbonementModel> abons = new List<ClientViewAbonementModel>();
            foreach (var abonement in abonements)
            {
                ClientViewAbonementModel abon = new ClientViewAbonementModel();
                abon.ID_abonement = abonement.ID_abonement;
                abon.Nazvanie = abonement.Usluga.Nazvanie;
                abon.Kolichestvo = abonement.Kolichestvo;
                abon.Data_nachala = abonement.Data_nachala;
                abon.Data_okonchania = abonement.Data_okonchania;

                if (abonement.Skidka != null)
                {
                    var procent = 100 / ((double) (100 - abonement.Skidka.Procent_value));
                    abon.Cena = abon.Kolichestvo * (abonement.Usluga.Cena_za_poseshenie * procent);
                }
                else
                    abon.Cena = abon.Kolichestvo * abonement.Usluga.Cena_za_poseshenie;

                var raspisanie = FitnesManager.Raspisanie_zanjatij.Where(r => r.ID_usluga == abonement.ID_usluga && r.Data >= abonement.Data_nachala).FirstOrDefault();
                if (raspisanie != null)
                    abon.Data_vizita = raspisanie.Data;

                abon.Sostojanie = abonement.Sostojanie;

                abons.Add(abon);
            }

            return View(abons);
        }

        // GET: Client/DetailsActiveAbonement
        [HttpGet]
        public ActionResult DetailsActiveAbonement()
        {
            ApplicationUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
            Klient klient = FitnesManager.Klients.Where(t => t.ID_Login == user.Id).FirstOrDefault();
            List<Abonement> abonements = new List<Abonement>();
            abonements = FitnesManager.Abonements.Where(c => c.ID_klient == klient.ID_klient).ToList();

            List<ClientViewAbonementModel> abons = new List<ClientViewAbonementModel>();
            foreach (var abonement in abonements)
            {
                ClientViewAbonementModel abon = new ClientViewAbonementModel();
                abon.Nazvanie = abonement.Usluga.Nazvanie;
                //abon.Kolichestvo = abonement.Kolichestvo;
                abon.Ostatok = abonement.Ostatok;
                abon.Data_nachala = abonement.Data_nachala;
                abon.Data_okonchania = abonement.Data_okonchania;

                if (abonement.Skidka != null)
                    abon.Procent = (double)abonement.Skidka.Procent_value;
                abon.Cena = abonement.Stoimost;

                var raspisanie = FitnesManager.Raspisanie_zanjatij.Where(r => r.ID_usluga == abonement.ID_usluga && r.Data >= abonement.Data_nachala).FirstOrDefault();
                if (raspisanie != null)
                    abon.Data_vizita = raspisanie.Data;

                abon.Sostojanie = abonement.Sostojanie;

                abons.Add(abon);
            }

            var activeAbonements = abons.Where(a => a.Sostojanie == "активный");

            return View(activeAbonements);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
            ViewBag.Uslugas = uslugas;


            return View();
        }

        // POST: Client/Create
        [HttpPost]
        public ActionResult Create(NewViewAbonementModel abonement)
        {
            ApplicationUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
            Klient klient = FitnesManager.Klients.Where(t => t.ID_Login == user.Id).FirstOrDefault();

            int ID_Usluga = Int32.Parse(abonement.Nazvanie);
            Usluga usluga = FitnesManager.Uslugas.Where(u => u.ID_usluga == ID_Usluga).FirstOrDefault();

            Abonement newAbonement = new Abonement
            {
                Klient = klient,
                ID_klient = klient.ID_klient,
                Usluga = usluga,
                ID_usluga = usluga.ID_usluga,
                Kolichestvo = abonement.Kolichestvo,
                Sostojanie = "заказаный"
            };

            FitnesManager.Abonements.Add(newAbonement);
            FitnesManager.SaveChanges();

            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Вывести текст ошибки
                ViewBag.ErrorMessage = ex.Message;
                SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
                ViewBag.Uslugas = uslugas;
                return View();
            }
        }

       // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            Abonement deleted = FitnesManager.Abonements.Find(id);

            FitnesManager.Entry(deleted).State = EntityState.Deleted;
            FitnesManager.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Client
        [AllowAnonymous]
        public ActionResult Raspisanie()
        {
            PeriodViewModel period = new PeriodViewModel();
            List<RaspisanieZanjatijViewModel> raspisanie = new List<RaspisanieZanjatijViewModel>();
            CalendarViewModel calendar = new CalendarViewModel();
            calendar.Period = period;
            calendar.Raspisanie = raspisanie;

            return View(calendar);
        }


        // GET: Client
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Raspisanie(CalendarViewModel periodCalendar)
        {
            CalendarViewModel calendar = new CalendarViewModel();
            calendar.Period = periodCalendar.Period;
            calendar.Raspisanie = new List<RaspisanieZanjatijViewModel>();

            var groupZanjaties = FitnesManager.Raspisanie_zanjatij.GroupBy(r => r.Data).ToList();

            //List<DateTime> dats = new List<DateTime>();
            DateTime countDat = returnNearestMonday(calendar.Period.Data_nachala);
            calendar.Period.Data_okonchania = returnNearestSunday(calendar.Period.Data_okonchania);
            while (countDat <= calendar.Period.Data_okonchania)
            {
                //dats.Add(countDat);

                RaspisanieZanjatijViewModel raspisanie = new RaspisanieZanjatijViewModel();

                var zanjaties = groupZanjaties.Find(z => z.Key == countDat);
                if (zanjaties != null)
                {
                    raspisanie.Data = zanjaties.Key;
                    raspisanie.Zanjaties = new List<string>();
                    foreach (var zanjatie in zanjaties.Where(z => z.Data == zanjaties.Key))
                    {
                        raspisanie.Zanjaties.Add(zanjatie.Usluga.Nazvanie);
                    }
                }
                else
                {
                    raspisanie.Data = countDat;
                    raspisanie.Zanjaties = new List<string>();
                }

                calendar.Raspisanie.Add(raspisanie);

                countDat = countDat.AddDays(1);
            }

      /*      foreach (var zanjaties in FitnesManager.Raspisanie_zanjatij.GroupBy(r => r.Data).ToList())
            {
                RaspisanieZanjatijViewModel raspisanie = new RaspisanieZanjatijViewModel();
                raspisanie.Data = zanjaties.Key;

                raspisanie.Zanjaties = new List<string>();
                foreach (var zanjatie in zanjaties.Where(z => z.Data == zanjaties.Key))
                {
                    raspisanie.Zanjaties.Add(zanjatie.Usluga.Nazvanie);
                }

                calendar.Raspisanie.Add(raspisanie);
            } 
      */
            return View(calendar);
        }

        #region Дополнительные методы
        DateTime returnNearestMonday(DateTime dateWeek)
        {
            while (dateWeek.DayOfWeek != System.DayOfWeek.Monday)
            {
                dateWeek = dateWeek.AddDays(-1);
            }
            return dateWeek;
        }

        DateTime returnNearestSunday(DateTime dateWeek)
        {
            while (dateWeek.DayOfWeek != System.DayOfWeek.Sunday)
            {
                dateWeek = dateWeek.AddDays(1);
            }
            return dateWeek;
        }
        #endregion
    }
}
