using Aplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Aplikacija.Models.ViewModeli;

namespace Aplikacija.Controllers
{
    [AllowAnonymous]
    public class PredbiljezbaController : Controller
    {
        public ActionResult Predbiljezba()
        {
            // Ako nema niti jednog registriranog zaposlenika (npr. prvo pokretanje aplikacije ili brisanje svih zaposlenika)
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                if (_db.Zaposlenik.Count() == 0)
                {
                    return RedirectToAction("PrvaRegistracija", "Zaposlenici");
                }
            }

            List<Seminar> seminari = new List<Seminar>();
            List<SeminarViewModel> seminariViewModel = new List<SeminarViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                seminari = _db.Seminar.Where(x => x.Objava).ToList();

                foreach (var item in seminari)
                {
                    SeminarViewModel seminarViewModel = new SeminarViewModel
                    {
                        IdSeminar = item.IdSeminar,
                        Naziv = item.Naziv,
                        Opis = item.Opis,
                        Datum = item.Datum,
                        MaxBrojPolaznika = item.MaxBrojPolaznika,
                        Popunjen = item.Popunjen,
                        UkupanBrojPredbiljezbi = item.Predbiljezbe.Count(), //dodajemo ukupan broj predbilježbi i šaljemo na popis seminara
                        BrojOdobrenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) //dodajemo broj odobrenih predbilježbi i šaljemo na popis seminara
                    };

                    seminariViewModel.Add(seminarViewModel);
                }
            }

            return View(seminariViewModel);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Predbiljezba(string pretraga)
        {
            List<Seminar> seminari = new List<Seminar>();
            List<SeminarViewModel> seminariViewModel = new List<SeminarViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                if (pretraga == "" || pretraga == null)
                {
                    seminari = _db.Seminar.Where(x => x.Objava).ToList();
                }
                else
                {
                    seminari = _db.Seminar.Where(x => x.Objava && x.Naziv.ToLower().Contains(pretraga.ToLower())).ToList();
                }

                foreach (var item in seminari)
                {
                    SeminarViewModel seminarViewModel = new SeminarViewModel
                    {
                        IdSeminar = item.IdSeminar,
                        Naziv = item.Naziv,
                        Opis = item.Opis,
                        Datum = item.Datum,
                        MaxBrojPolaznika = item.MaxBrojPolaznika,
                        Popunjen = item.Popunjen,
                        UkupanBrojPredbiljezbi = item.Predbiljezbe.Count(), //dodajemo ukupan broj predbilježbi i šaljemo na popis seminara
                        BrojOdobrenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) //dodajemo broj odobrenih predbilježbi i šaljemo na popis seminara
                    };

                    seminariViewModel.Add(seminarViewModel);
                }

            }

            return View(seminariViewModel);
        }

        public ActionResult NovaPredbiljezba(int id)
        {
            Predbiljezba predbiljezba = new Predbiljezba
            {
                IdSeminar = id,
                Datum = DateTime.Now
            };

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                ViewBag.NazivSeminara = _db.Seminar.Where(x => x.IdSeminar == id).Select(n => n.Naziv).FirstOrDefault();
            }

            return View(predbiljezba);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult NovaPredbiljezba(Predbiljezba predbiljezba)
        {

            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Predbiljezba.Add(predbiljezba);
                    db.SaveChanges();
                    ViewBag.Message = predbiljezba.Ime + " " + predbiljezba.Prezime + ", vaša predbilježba je uspješno zaprimljena! Zahvaljujemo.";
                    ViewBag.NazivSeminara = db.Seminar.Where(x => x.IdSeminar == predbiljezba.IdSeminar).Select(n => n.Naziv).FirstOrDefault();
                }

                ModelState.Clear();
            }

            return View();
        }
    }
}