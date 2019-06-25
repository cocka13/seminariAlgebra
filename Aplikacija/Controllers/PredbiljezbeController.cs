using Aplikacija.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using static Aplikacija.Models.ViewModeli;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class PredbiljezbeController : Controller
    {
        public ActionResult Predbiljezbe()
        {
            List<Predbiljezba> predbiljezbe = new List<Predbiljezba>();
            List<PredbiljezbaViewModel> predbiljezbeViewModel = new List<PredbiljezbaViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                predbiljezbe = _db.Predbiljezba.ToList();

                foreach (var item in predbiljezbe)
                {
                    PredbiljezbaViewModel predbiljezbaViewModel = new PredbiljezbaViewModel
                    {
                        IdPredbiljezba = item.IdPredbiljezba,
                        IdSeminar = item.IdSeminar,
                        Datum = item.Datum,
                        Ime = item.Ime,
                        Prezime = item.Prezime,
                        Adresa = item.Adresa,
                        Email = item.Email,
                        Telefon = item.Telefon,
                        StatusPredbiljezbe = item.StatusPredbiljezbe,
                        NazivSeminara = _db.Seminar.Where(x => x.IdSeminar == item.IdSeminar).Select(x => x.Naziv).FirstOrDefault()
                    };

                    predbiljezbeViewModel.Add(predbiljezbaViewModel);
                }
            }

            return View(predbiljezbeViewModel);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Predbiljezbe(string pretraga, string status)
        {
            List<Predbiljezba> predbiljezbe = new List<Predbiljezba>();
            List<PredbiljezbaViewModel> predbiljezbeViewModel = new List<PredbiljezbaViewModel>();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                if (pretraga == "")
                {
                    predbiljezbe = _db.Predbiljezba.ToList();
                }
                else
                {
                    predbiljezbe = _db.Predbiljezba.Where(x => x.Seminar.Naziv.ToLower().Contains(pretraga.ToLower())).ToList();
                }

                if (status != "")
                {
                    predbiljezbe = predbiljezbe.Where(x => x.StatusPredbiljezbe.ToString() == status).ToList();
                }

                foreach (var item in predbiljezbe)
                {
                    PredbiljezbaViewModel predbiljezbaViewModel = new PredbiljezbaViewModel
                    {
                        IdPredbiljezba = item.IdPredbiljezba,
                        IdSeminar = item.IdSeminar,
                        Datum = item.Datum,
                        Ime = item.Ime,
                        Prezime = item.Prezime,
                        Adresa = item.Adresa,
                        Email = item.Email,
                        Telefon = item.Telefon,
                        StatusPredbiljezbe = item.StatusPredbiljezbe,
                        NazivSeminara = _db.Seminar.Where(x => x.IdSeminar == item.IdSeminar).Select(x => x.Naziv).FirstOrDefault()
                    };

                    predbiljezbeViewModel.Add(predbiljezbaViewModel);
                }
            }

            return View(predbiljezbeViewModel);
        }

        public ActionResult UrediPredbiljezbu(int? id)
        {

            Predbiljezba predbiljezba = new Predbiljezba();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                predbiljezba = _db.Predbiljezba.Where(x => x.IdPredbiljezba == id).FirstOrDefault();
                ViewBag.NazivSeminara = _db.Predbiljezba.Where(x => x.IdPredbiljezba == id).Select(n => n.Seminar.Naziv).FirstOrDefault();
                ViewBag.Seminari = _db.Seminar.ToList();
            }

            return View(predbiljezba);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult UrediPredbiljezbu(Predbiljezba predbiljezba)
        {
            Predbiljezba predbiljezbaPromjena = new Predbiljezba();
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    predbiljezbaPromjena = db.Predbiljezba.Find(predbiljezba.IdPredbiljezba);
                    predbiljezbaPromjena.IdSeminar = predbiljezba.IdSeminar;
                    predbiljezbaPromjena.Datum = predbiljezba.Datum;
                    predbiljezbaPromjena.Ime = predbiljezba.Ime;
                    predbiljezbaPromjena.Prezime = predbiljezba.Prezime;
                    predbiljezbaPromjena.Adresa = predbiljezba.Adresa;
                    predbiljezbaPromjena.Email = predbiljezba.Email;
                    predbiljezbaPromjena.Telefon = predbiljezba.Telefon;
                    predbiljezbaPromjena.StatusPredbiljezbe = predbiljezba.StatusPredbiljezbe;

                    db.Entry(predbiljezbaPromjena).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Message = "Predbilježba je uspješno promijenjena.";
                    ViewBag.Seminari = db.Seminar.ToList();
                }
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ViewBag.NazivSeminara = db.Predbiljezba.Where(x => x.IdPredbiljezba == predbiljezba.IdPredbiljezba).Select(n => n.Seminar.Naziv).FirstOrDefault();
                    ViewBag.Seminari = db.Seminar.ToList();
                    return View(predbiljezba);
                }                
            }

            ModelState.Clear();
            return RedirectToAction("Predbiljezbe");
        }

        public ActionResult ObrisiPredbiljezbu(int? id)
        {
            Predbiljezba predbiljezba = new Predbiljezba();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                predbiljezba = _db.Predbiljezba.Find(id);
                ViewBag.NazivSeminara = _db.Predbiljezba.Where(x => x.IdPredbiljezba == id).Select(x => x.Seminar.Naziv).FirstOrDefault();
            }

            return View(predbiljezba);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult ObrisiPredbiljezbu(int id)
        {
            Predbiljezba predbiljezba = new Predbiljezba();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                predbiljezba = db.Predbiljezba.Find(id);
                db.Predbiljezba.Remove(predbiljezba);
                db.SaveChanges();
            }

            return RedirectToAction("Predbiljezbe");
        }
    }
}