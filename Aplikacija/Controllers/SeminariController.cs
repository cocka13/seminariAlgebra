using Aplikacija.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using static Aplikacija.Models.ViewModeli;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class SeminariController : Controller
    {
        public ActionResult Seminari()
        {
            List<Seminar> seminari = new List<Seminar>();
            List<SeminarViewModel> seminariViewModel = new List<SeminarViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                seminari = _db.Seminar.ToList();

                foreach (var item in seminari)
                {
                    SeminarViewModel seminarViewModel = new SeminarViewModel();

                    seminarViewModel.IdSeminar = item.IdSeminar;
                    seminarViewModel.Objava = item.Objava;
                    seminarViewModel.Naziv = item.Naziv;
                    seminarViewModel.Opis = item.Opis;
                    seminarViewModel.Datum = item.Datum;
                    seminarViewModel.MaxBrojPolaznika = item.MaxBrojPolaznika;
                    seminarViewModel.Popunjen = item.Popunjen;
                    seminarViewModel.UkupanBrojPredbiljezbi = item.Predbiljezbe.Count(); //dodajemo ukupan broj predbilježbi i šaljemo na popis seminara
                    seminarViewModel.BrojOdobrenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena); //dodajemo broj odobrenih predbilježbi i šaljemo na popis seminara
                    seminarViewModel.BrojNeobradjenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Neobradjena); //dodajemo broj neobrađenih predbilježbi i šaljemo na popis seminara

                    seminariViewModel.Add(seminarViewModel);
                }
            }

            return View(seminariViewModel);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Seminari(string pretraga, bool? popunjenost, bool? objava)
        {
            List<Seminar> seminari = new List<Seminar>();
            List<SeminarViewModel> seminariViewModel = new List<SeminarViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                if (pretraga == null || pretraga == "")
                {
                    seminari = _db.Seminar.ToList();
                }
                else
                {
                    seminari = _db.Seminar.Where(x => x.Naziv.ToLower().Contains(pretraga.ToLower())).ToList();
                }

                if (popunjenost != null)
                {
                    seminari = seminari.Where(x => x.Popunjen == popunjenost).ToList();
                }

                if (objava != null)
                {
                    seminari = seminari.Where(x => x.Objava == objava).ToList();
                }

                foreach (var item in seminari)
                {
                    SeminarViewModel seminarViewModel = new SeminarViewModel();

                    seminarViewModel.IdSeminar = item.IdSeminar;
                    seminarViewModel.Objava = item.Objava;
                    seminarViewModel.Naziv = item.Naziv;
                    seminarViewModel.Opis = item.Opis;
                    seminarViewModel.Datum = item.Datum;
                    seminarViewModel.MaxBrojPolaznika = item.MaxBrojPolaznika;
                    seminarViewModel.Popunjen = item.Popunjen;
                    seminarViewModel.UkupanBrojPredbiljezbi = item.Predbiljezbe.Count(); //dodajemo ukupan broj predbilježbi i šaljemo na popis seminara
                    seminarViewModel.BrojOdobrenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena); //dodajemo broj odobrenih predbilježbi i šaljemo na popis seminara
                    seminarViewModel.BrojNeobradjenihPredbiljezbi = item.Predbiljezbe.Count(m => m.StatusPredbiljezbe == EnumStatusPredbiljezbe.Neobradjena); //dodajemo broj neobrađenih predbilježbi i šaljemo na popis seminara

                    seminariViewModel.Add(seminarViewModel);
                }
            }

            return View(seminariViewModel);
        }

        public ActionResult AzurirajPopunjenost(int? id)
        {
            List<Seminar> seminari = new List<Seminar>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                foreach (var item in _db.Seminar.ToList())
                {
                    if (item.Predbiljezbe.Count(x => x.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) >= item.MaxBrojPolaznika && item.Popunjen == false)
                    {
                        seminari.Add(item);
                    }

                    if (item.Predbiljezbe.Count(x => x.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) < item.MaxBrojPolaznika && item.Popunjen == true)
                    {
                        seminari.Add(item);
                    }
                }
            }

            if (seminari.Count == 0)
            {
                ViewBag.Message = "Nema seminara za ažurirati popunjenost";
            }

            return View(seminari);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajPopunjenost()
        {
            List<Seminar> seminari = new List<Seminar>();
            Seminar seminar = new Seminar();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var item in db.Seminar.ToList())
                {
                    if (item.Predbiljezbe.Count(x => x.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) >= item.MaxBrojPolaznika && item.Popunjen == false)
                    {
                        seminari.Add(item);
                    }
                }

                foreach (var item in seminari)
                {
                    seminar = db.Seminar.Find(item.IdSeminar);
                    seminar.IdSeminar = item.IdSeminar;
                    seminar.Objava = item.Objava;
                    seminar.Naziv = item.Naziv;
                    seminar.Opis = item.Opis;
                    seminar.Datum = item.Datum;
                    seminar.MaxBrojPolaznika = item.MaxBrojPolaznika;
                    seminar.Popunjen = true;

                    db.Entry(seminar).State = EntityState.Modified;
                    db.SaveChanges();
                }

                seminari.Clear();

                foreach (var item in db.Seminar.ToList())
                {
                    if (item.Predbiljezbe.Count(x => x.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena) < item.MaxBrojPolaznika && item.Popunjen == true)
                    {
                        seminari.Add(item);
                    }
                }

                foreach (var item in seminari)
                {
                    seminar = db.Seminar.Find(item.IdSeminar);
                    seminar.IdSeminar = item.IdSeminar;
                    seminar.Objava = item.Objava;
                    seminar.Naziv = item.Naziv;
                    seminar.Opis = item.Opis;
                    seminar.Datum = item.Datum;
                    seminar.MaxBrojPolaznika = item.MaxBrojPolaznika;
                    seminar.Popunjen = false;

                    db.Entry(seminar).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Seminari");
        }

        public ActionResult UrediSeminar(int? id)
        {
            Seminar seminar = new Seminar();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                seminar = _db.Seminar.Where(x => x.IdSeminar == id).FirstOrDefault();
            }

            return View(seminar);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult UrediSeminar(Seminar seminar)
        {
            Seminar seminarPromjena = new Seminar();

            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    seminarPromjena = db.Seminar.Find(seminar.IdSeminar);
                    int brojPrihvacenihPredbiljezbi = db.Predbiljezba.Where(x => x.Seminar.IdSeminar == seminar.IdSeminar).Count(x => x.StatusPredbiljezbe == EnumStatusPredbiljezbe.Prihvacena);

                    if (seminar.MaxBrojPolaznika == brojPrihvacenihPredbiljezbi && seminar.Popunjen == false)
                    {
                        ViewBag.Message = "Maksimalan broj polaznika je jednak broju prihvaćenih predbilježbi. Molimo označite da je seminar popunjen ili povećajte broj polaznika.";
                        return View(seminar);
                    }

                    if (seminar.MaxBrojPolaznika < brojPrihvacenihPredbiljezbi)
                    {
                        ViewBag.Message = "Maksimalan broj polaznika je manji od broja prihvaćenih predbilježbi. Molimo povećajte broj polaznika.";
                        return View(seminar);
                    }

                    if ((seminar.MaxBrojPolaznika > brojPrihvacenihPredbiljezbi) || (seminar.MaxBrojPolaznika == brojPrihvacenihPredbiljezbi && seminar.Popunjen == true))
                    {
                        seminarPromjena.IdSeminar = seminar.IdSeminar;
                        seminarPromjena.Objava = seminar.Objava;
                        seminarPromjena.Naziv = seminar.Naziv;
                        seminarPromjena.Opis = seminar.Opis;
                        seminarPromjena.Datum = seminar.Datum;
                        seminarPromjena.MaxBrojPolaznika = seminar.MaxBrojPolaznika;

                        if (seminar.MaxBrojPolaznika > brojPrihvacenihPredbiljezbi && seminarPromjena.Popunjen == true)
                        {
                            seminarPromjena.Popunjen = false;
                        }
                        else
                        {
                            seminarPromjena.Popunjen = seminar.Popunjen;
                        }

                        db.Entry(seminarPromjena).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                return View(seminar);
            }

            ModelState.Clear();
            return RedirectToAction("Seminari");
        }

        public ActionResult NoviSeminar()
        {

            return View();
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult NoviSeminar(Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Seminar.Add(seminar);
                    db.SaveChanges();
                }
            }
            else
            {
                return View(seminar);
            }

            ModelState.Clear();

            return RedirectToAction("Seminari");
        }

        public ActionResult ObrisiSeminar(int? id)
        {
            Seminar seminar = new Seminar();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                seminar = _db.Seminar.Find(id);
            }

            return View(seminar);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult ObrisiSeminar(int id)
        {
            Seminar seminar = new Seminar();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                seminar = db.Seminar.Find(id);
                db.Seminar.Remove(seminar);
                db.SaveChanges();
            }

            return RedirectToAction("Seminari");
        }
    }
}