using Fitness.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private Entities _fitnesManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, Entities fitnesManager)
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

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Admin/DetailsUslugas
        public ActionResult DetailsUslugas()
        {
            List<BonusUslugaViewModel> bonusUslugas = new List<BonusUslugaViewModel>();
            var uslugas = FitnesManager.Uslugas.ToList();

            foreach (var usl in uslugas)
            {
                BonusUslugaViewModel bonus = new BonusUslugaViewModel();
                bonus.ID_usluga = usl.ID_usluga;
                bonus.Nazvanie = usl.Nazvanie;
                bonus.Cena_za_poseshenie = usl.Cena_za_poseshenie;
                var skidkas = usl.Skidkas.Select(t => t.Procent_value.ToString() + "%, " + t.Kolichestvo.ToString() + " дней").ToList();
                bonus.Skidkas = skidkas;             

                bonusUslugas.Add(bonus);
            }

            return View(bonusUslugas);
        }

        // GET: Admin/Edit/5
        public ActionResult CreateSkidka(int id)
        {
            SkidkaViewModel skidka = new SkidkaViewModel();
            skidka.Nazvanie_uslugi = FitnesManager.Uslugas.Find(id).Nazvanie;

            return View(skidka);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult CreateSkidka(int id, SkidkaViewModel skidka)
        {
            try
            {
                // TODO: Add update logic here
                var usluga = FitnesManager.Uslugas.Find(id);
                var newskidka = new Skidka
                {
                    Usluga = usluga,
                    ID_usluga = usluga.ID_usluga,
                    Kolichestvo = skidka.Kolichestvo,
                    Procent_value = (decimal)skidka.Procent_value
                };

                FitnesManager.Skidkas.Add(newskidka);
                FitnesManager.SaveChanges();

                return RedirectToAction("DetailsUslugas");
            }
            catch
            {
                return View(skidka);
            }
        }

        public ActionResult DetailsRaspisanie()
        {
            List<RaspisanieViewModel> viewRaspisanie = new List<RaspisanieViewModel>();
            var raspisanie = FitnesManager.Raspisanie_zanjatij.ToList();

            foreach (var rasp in raspisanie)
            {
                RaspisanieViewModel view = new RaspisanieViewModel();
                view.ID_zanjatia = rasp.ID_zanjatia;
                view.Data = rasp.Data;
                view.Usluga = rasp.Usluga.Nazvanie;

                if (rasp.Sotrudnik != null)
                    view.Sotrudnik = rasp.Sotrudnik.FIO_sotrudnik;
                if (rasp.Pomeshenie != null)
                    view.Pomeshenie = rasp.Pomeshenie.Nazvanie_pomeshenia;
                if (rasp.Poseshenies != null && rasp.Poseshenies.Count > 0)
                    view.IsVisit = true;

                viewRaspisanie.Add(view);
            }

            return View(viewRaspisanie.OrderBy(r => r.Data).ToList());
        }

        // GET: Admin/Edit/5
        public ActionResult CreateZanjatie()
        {
            SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
            ViewBag.Uslugas = uslugas;
            SelectList sotrudniks = new SelectList(
                FitnesManager.Sotrudniks.Where(s=> s.Dolgnost.Nazvanie_dolgnosti != "Администратор").ToList(), "ID_sotrudnik", "FIO_sotrudnik");
            ViewBag.Sotrudniks = sotrudniks;
            SelectList pomeshenies = new SelectList(FitnesManager.Pomeshenies, "ID_pomeshenia", "Nazvanie_pomeshenia");
            ViewBag.Pomeshenies = pomeshenies;

            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult CreateZanjatie(RaspisanieViewModel zanjatie)
        {
            int value = Int32.Parse(zanjatie.Usluga);
            var usluga = FitnesManager.Uslugas.Where(u => u.ID_usluga == value).FirstOrDefault();
            Int32.TryParse(zanjatie.Sotrudnik, out value);
            var sotrudnik = FitnesManager.Sotrudniks.Where(s => s.ID_sotrudnik == value).FirstOrDefault();
            Int32.TryParse(zanjatie.Pomeshenie, out value);
            var pomeshenie = FitnesManager.Pomeshenies.Where(p => p.ID_pomeshenia == value).FirstOrDefault();

            Raspisanie_zanjatij newzanjatie;
            if (sotrudnik != null && pomeshenie!= null)
            {
                newzanjatie = new Raspisanie_zanjatij
                {
                    Usluga = usluga,
                    ID_usluga = usluga.ID_usluga,
                    Data = zanjatie.Data,
                    Sotrudnik = sotrudnik,
                    ID_sotrudnik = sotrudnik.ID_sotrudnik,
                    Pomeshenie = pomeshenie,
                    ID_pomeshenia = pomeshenie.ID_pomeshenia
                };
            }
            else
            {
                newzanjatie = new Raspisanie_zanjatij
                {
                    Usluga = usluga,
                    ID_usluga = usluga.ID_usluga,
                    Data = zanjatie.Data,
                    Sotrudnik = sotrudnik,
                    Pomeshenie = pomeshenie,
                };
            }

            FitnesManager.Raspisanie_zanjatij.Add(newzanjatie);
            FitnesManager.SaveChanges();

            try
            {
                // TODO: Add update logic here
               

                return RedirectToAction("DetailsRaspisanie");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
                ViewBag.Uslugas = uslugas;
                SelectList sotrudniks = new SelectList(FitnesManager.Sotrudniks, "ID_sotrudnik", "FIO_sotrudnik");
                ViewBag.Sotrudniks = sotrudniks;
                SelectList pomeshenies = new SelectList(FitnesManager.Pomeshenies, "ID_pomeshenia", "Nazvanie_pomeshenia");
                ViewBag.Pomeshenies = pomeshenies;

                return View(zanjatie);
            }
        }

        // GET: Admin/CheckPoseshenie/5
        [HttpGet]
        public ActionResult CheckPoseshenie(int id)
        {
            var zanjatie = FitnesManager.Raspisanie_zanjatij.Find(id);

            CheckZanjatieViewModel checkZanjatie = new CheckZanjatieViewModel();
            checkZanjatie.ID_zanjatia = zanjatie.ID_zanjatia;
            checkZanjatie.Data = zanjatie.Data;
            checkZanjatie.Usluga = zanjatie.Usluga.Nazvanie;

            checkZanjatie.klients = new List<VisitedClientViewModel>();
            foreach (var abonement in FitnesManager.Abonements
                                   .Where(a => a.ID_usluga == zanjatie.ID_usluga && a.Sostojanie == "активный"))
            {
                VisitedClientViewModel visitKlient = new VisitedClientViewModel();
                visitKlient.ID_abonement = abonement.ID_abonement;
                visitKlient.Klient = abonement.Klient.FIO_klient;

                checkZanjatie.klients.Add(visitKlient);
            }

            return View(checkZanjatie);
        }

        // POST: Admin/CheckPoseshenie/5
        [HttpPost]
        public ActionResult CheckPoseshenie(int id, CheckZanjatieViewModel checkZanjatie)
        {

            var checkedZanjatie = FitnesManager.Raspisanie_zanjatij.Find(id);

            foreach (var klient in checkZanjatie.klients)
            {
                if (klient.Visit)
                {
                    Abonement abonement = FitnesManager.Abonements.Find(klient.ID_abonement);

                    // decrement Ostatok
                    abonement.Ostatok -= 1;

                    if (abonement.Ostatok == 0)
                    {
                        abonement.Sostojanie = "использованый";
                        abonement.Data_okonchania = DateTime.Today;
                    }

                    FitnesManager.Entry(abonement).State = EntityState.Modified;

                    Poseshenie poseshenie = new Poseshenie();
                    poseshenie.Raspisanie_zanjatij = checkedZanjatie;
                    poseshenie.Abonement = abonement;
                    FitnesManager.Poseshenies.Add(poseshenie);

                    // nachisl zarplatu sotrudnikam
                    if (checkedZanjatie.Sotrudnik != null)
                    {
                        Zarplata_instruktoram zpInstructoram = new Zarplata_instruktoram();
                        zpInstructoram.Poseshenie = poseshenie;
                        zpInstructoram.Usluga = poseshenie.Abonement.Usluga;
                        zpInstructoram.Summa = checkedZanjatie.Usluga.Cena_za_poseshenie * 0.8;

                        FitnesManager.Zarplata_instruktoram.Add(zpInstructoram);
                    }
                    if (poseshenie.Abonement.Sostojanie == "использованый" && poseshenie.Abonement.Sotrudnik != null)
                    {
                        Zarplata_administratoram zpAdministratoram = new Zarplata_administratoram();
                        zpAdministratoram.Abonement = poseshenie.Abonement;
                        zpAdministratoram.Summa = checkedZanjatie.Usluga.Cena_za_poseshenie * 0.8;

                        FitnesManager.Zarplata_administratoram.Add(zpAdministratoram);
                    }
                }                
            }

            FitnesManager.SaveChanges();

            return RedirectToAction("DetailsRaspisanie");
        }

        public ActionResult DetailsPoseshenies()
        {
            List<PosesheniesViewModel> viewPoseshenies = new List<PosesheniesViewModel>();
            var poseshenies = FitnesManager.Poseshenies.ToList();
            var poses = poseshenies.GroupBy(p => new { p.Raspisanie_zanjatij.Data, p.Abonement.Usluga.Nazvanie }).ToList();
            foreach (var poseshenie in poses)
            {
                PosesheniesViewModel pos = new PosesheniesViewModel();
                pos.Data = poseshenie.Key.Data;  
                pos.Usluga = poseshenie.Key.Nazvanie;

                pos.klients = new List<string>();
                foreach (var klient in poseshenie)
                {
                    pos.klients.Add(klient.Abonement.Klient.FIO_klient);
                }
                pos.Kolichestvo = poseshenie.Count();

                viewPoseshenies.Add(pos);
            }

            return View(viewPoseshenies);
        }

        public ActionResult DetailsAbonements()
        {
            List<AdminViewAbonementModel> viewAbonements = new List<AdminViewAbonementModel>();
            var abonements = FitnesManager.Abonements.ToList();

            foreach (var abon in abonements)
            {
                AdminViewAbonementModel view = new AdminViewAbonementModel();
                view.ID_abonement = abon.ID_abonement;
                view.Klient_FIO = abon.Klient.FIO_klient;
                view.Nazvanie = abon.Usluga.Nazvanie;
                view.Kolichestvo = abon.Kolichestvo;

                if (abon.Skidka != null)
                    view.Procent = (double)abon.Skidka.Procent_value;
                view.Stoimost = abon.Stoimost;
                if (abon.Sostojanie != "заказаный")
                    view.Ostatok = abon.Ostatok;

                view.Data_nachala = abon.Data_nachala;
                view.Data_okonchania = abon.Data_okonchania;

                view.Sostojanie = abon.Sostojanie;

                viewAbonements.Add(view);
            }

            return View(viewAbonements);
        }

        // GET: Admin/CreateClient
        public ActionResult CreateKlient()
        {
            CreateKlientViewModel klient = new CreateKlientViewModel();

            SelectList ulicas = new SelectList(FitnesManager.Ulicas, "ID_ulica", "Nazvanie_ulicu");
            ViewBag.Ulicas = ulicas;

            return View(klient);
        }

        // POST: Admin/CreateClient
        [HttpPost]
        public ActionResult CreateKlient(CreateKlientViewModel klient)
        {        
            try
            {
                Klient newKlient = new Klient();

                newKlient.FIO_klient = klient.FIO;
                newKlient.Telephon = klient.Telephon;

                Adress adress = new Adress();
                int ID_ulica;
                Int32.TryParse(klient.Ulica, out ID_ulica);
                Ulica ulica = FitnesManager.Ulicas.Find(ID_ulica);
                if (ulica != null)
                    adress = new Adress { Ulica = ulica, ID_ulica = ulica.ID_ulica, Nomer_doma = klient.Nomer_doma, Nomer_kvartiru = klient.Nomer_kvartiru };
                else
                {
                    ulica = new Ulica { Nazvanie_ulicu = klient.Ulica };
                    FitnesManager.Ulicas.Add(ulica);
                    adress = new Adress { Ulica = ulica, ID_ulica = ulica.ID_ulica, Nomer_doma = klient.Nomer_doma, Nomer_kvartiru = klient.Nomer_kvartiru };
                }
                FitnesManager.Adresses.Add(adress);
                newKlient.Adress = adress;

                FitnesManager.Klients.Add(newKlient);
                FitnesManager.SaveChanges();

                return RedirectToAction("CreateAbonement", new { id = newKlient.ID_klient });
            }
            catch (Exception ex)
            {
                SelectList ulicas = new SelectList(FitnesManager.Ulicas, "ID_ulica", "Nazvanie_ulicu");
                ViewBag.Ulicas = ulicas;

                return View(klient);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult PassCreateKlient()
        {        
            return RedirectToAction("CreateAbonement", new { id = FitnesManager.Klients.First().ID_klient });
        }

        // GET: Admin/Edit/5
        public ActionResult CreateAbonement(int id)
        {
            EditAbonementViewModel edited = new EditAbonementViewModel();

            var klient = FitnesManager.Klients.Find(id);
            SelectList klients = new SelectList(FitnesManager.Klients, "ID_klient", "FIO_klient", klient.ID_klient);
            ViewBag.Klients = klients;

            SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
            ViewBag.Uslugas = uslugas;

            var skidkas = FitnesManager.Skidkas;
            SelectList skidka = new SelectList(skidkas, "ID_skidka", "Procent_value");
            ViewBag.Skidkas = skidka;

            return View(edited);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult CreateAbonement(int id, EditAbonementViewModel editedAbon)
        {
            try
            {
                // TODO: Add update logic here
                var abonement = new Abonement();

                int ID_klient;
                Int32.TryParse(editedAbon.Klient_FIO, out ID_klient);
                abonement.Klient = FitnesManager.Klients.Find(ID_klient);

                int ID_usluga;
                Int32.TryParse(editedAbon.Nazvanie, out ID_usluga);
                abonement.Usluga = FitnesManager.Uslugas.Find(ID_usluga);

                abonement.Kolichestvo = editedAbon.Kolichestvo;
                abonement.Ostatok = editedAbon.Kolichestvo;

                if (editedAbon.Skidka != null)
                {
                    int ID_skidka;
                    Int32.TryParse(editedAbon.Skidka, out ID_skidka);
                    var skidka = FitnesManager.Skidkas.Find(ID_skidka);
                    abonement.Skidka = skidka;

                    var procent = 100 / ((double)(100 - abonement.Skidka.Procent_value));
                    abonement.Stoimost = abonement.Kolichestvo * (abonement.Usluga.Cena_za_poseshenie * procent);
                }
                else
                    abonement.Stoimost = abonement.Kolichestvo * abonement.Usluga.Cena_za_poseshenie;

                abonement.Data_nachala = editedAbon.Data_nachala;
                abonement.Data_okonchania = editedAbon.Data_okonchania;

                abonement.Sostojanie = "активный";

                FitnesManager.Abonements.Add(abonement);

                FitnesManager.SaveChanges();

                return RedirectToAction("DetailsAbonements");
            }
            catch
            {

                EditAbonementViewModel edited = new EditAbonementViewModel();

                var klient = FitnesManager.Klients.Find(id);
                SelectList klients = new SelectList(FitnesManager.Klients, "ID_klient", "FIO_klient", klient.ID_klient);
                ViewBag.Klients = klients;

                SelectList uslugas = new SelectList(FitnesManager.Uslugas, "ID_usluga", "Nazvanie");
                ViewBag.Uslugas = uslugas;

                var skidkas = FitnesManager.Skidkas;
                SelectList skidka = new SelectList(skidkas, "ID_skidka", "Procent_value");
                ViewBag.Skidkas = skidka;

                return View(edited);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult EditAbonement(int id)
        {
            EditAbonementViewModel edited = new EditAbonementViewModel();
            var abonement = FitnesManager.Abonements.Find(id);
            edited.Klient_FIO = abonement.Klient.FIO_klient;
            edited.Nazvanie = abonement.Usluga.Nazvanie;
            edited.Kolichestvo = abonement.Kolichestvo;

            var skidkas = FitnesManager.Skidkas
                 .Where(s => s.ID_usluga == abonement.ID_usluga && s.Kolichestvo == abonement.Kolichestvo);

            SelectList skidka = new SelectList(skidkas, "ID_skidka", "Procent_value");
            ViewBag.Skidkas = skidka;

            return View(edited);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditAbonement(int id, EditAbonementViewModel editedAbon)
        {
            try
            {
                // TODO: Add update logic here
                var abonement = FitnesManager.Abonements.Find(id);
                abonement.Kolichestvo = editedAbon.Kolichestvo;
                abonement.Ostatok = editedAbon.Kolichestvo;

                if (editedAbon.Skidka != null)
                {
                    int ID_skidka;
                    Int32.TryParse(editedAbon.Skidka, out ID_skidka);
                    var skidka = FitnesManager.Skidkas.Find(ID_skidka);
                    abonement.Skidka = skidka;

                    var procent = 100 / ((double)(100 - abonement.Skidka.Procent_value));
                    abonement.Stoimost = abonement.Kolichestvo * (abonement.Usluga.Cena_za_poseshenie * procent);
                }
                else
                    abonement.Stoimost = abonement.Kolichestvo * abonement.Usluga.Cena_za_poseshenie;

                abonement.Data_nachala = editedAbon.Data_nachala;
                abonement.Data_okonchania = editedAbon.Data_okonchania;

                abonement.Sostojanie = "активный";

                FitnesManager.Entry(abonement).State = EntityState.Modified;

                FitnesManager.SaveChanges();

                return RedirectToAction("DetailsAbonements");
            }
            catch
            {

                EditAbonementViewModel edited = new EditAbonementViewModel();
                var abonement = FitnesManager.Abonements.Find(id);
                edited.Klient_FIO = abonement.Klient.FIO_klient;
                edited.Nazvanie = abonement.Usluga.Nazvanie;
                edited.Kolichestvo = abonement.Kolichestvo;

                var skidkas = FitnesManager.Skidkas
                     .Where(s => s.ID_usluga == abonement.ID_usluga && s.Kolichestvo == abonement.Kolichestvo);

                SelectList skidka = new SelectList(skidkas, "ID_skidka", "Procent_value");
                ViewBag.Skidkas = skidka;

                return View(editedAbon);
            }
        }

        public ActionResult DetailsSotrudniks()
        {

            List<SotrudnikViewModel> viewSotrudniks = new List<SotrudnikViewModel>();
            var sotrudniks = FitnesManager.Sotrudniks.ToList();

            foreach (var sotr in sotrudniks)
            {
                SotrudnikViewModel view = new SotrudnikViewModel();
                view.FIO_sotrudnik = sotr.FIO_sotrudnik;
                view.Pasport = sotr.Pasport;
                view.Telephon = sotr.Telephon;

                if (sotr.Adress != null)
                {
                    view.Adress = sotr.Adress.Ulica.Nazvanie_ulicu + ", " + sotr.Adress.Nomer_doma.ToString();
                    if (sotr.Adress.Nomer_kvartiru != null)
                        view.Adress += " кв. " + sotr.Adress.Nomer_kvartiru.ToString();
                }

                view.Dolgnost = sotr.Dolgnost.Nazvanie_dolgnosti;

                viewSotrudniks.Add(view);
            }

            return View(viewSotrudniks);
        }

        public ActionResult DetailsZarplata()
        {

            ListZarplataViewModel viewZarplata = new ListZarplataViewModel();

            viewZarplata.Data_okonchania = DateTime.Today;
            viewZarplata.Data_nachala = viewZarplata.Data_okonchania.AddDays(-7);

            var sotrudniks = FitnesManager.Sotrudniks.ToList();
            foreach (var sotr in sotrudniks)
            {
                ZarplataViewModel zarplata = new ZarplataViewModel();
                if (sotr.Dolgnost.Nazvanie_dolgnosti == "Администратор")
                {
                    zarplata.FIO_sotrudnik = sotr.FIO_sotrudnik;
                    zarplata.Dolgnost = sotr.Dolgnost.Nazvanie_dolgnosti;

                    var groupZP = FitnesManager.Zarplata_administratoram.GroupBy(z => z.Abonement.ID_sotrudnik);
                    var zp = groupZP.Where(g => g.Key == sotr.ID_sotrudnik)
                                    .Select(z => new { Sotr = z.Key, Summa = z.Sum(s => s.Summa)}).ToList();
                    if (zp.Find(z => z.Sotr == sotr.ID_sotrudnik) != null)
                        zarplata.Summa = zp.Find(z => z.Sotr == sotr.ID_sotrudnik).Summa;

                }
                else
                {
                    zarplata.FIO_sotrudnik = sotr.FIO_sotrudnik;
                    zarplata.Dolgnost = sotr.Dolgnost.Nazvanie_dolgnosti;

                    var groupZP = FitnesManager.Zarplata_instruktoram.GroupBy(z => z.Poseshenie.Raspisanie_zanjatij.ID_sotrudnik);
                    var zp = groupZP.Where(g => g.Key == sotr.ID_sotrudnik)
                                    .Select(z => new { Sotr = z.Key, Summa = z.Sum(s => s.Summa) }).ToList();
                    if (zp.Find(z => z.Sotr == sotr.ID_sotrudnik) != null)
                        zarplata.Summa = zp.Find(z => z.Sotr == sotr.ID_sotrudnik).Summa;
                }
                viewZarplata.Zarplats.Add(zarplata);
            }

            return View(viewZarplata);
        }
    }
}
