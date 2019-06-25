using Aplikacija.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Aplikacija.Controllers
{
    [Authorize]
    public class ZaposleniciController : Controller
    {
        public ActionResult Zaposlenici()
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return View(_db.Zaposlenik.ToList());
            }
        }

        [AllowAnonymous]
        public ActionResult Prijava()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Prijava(Zaposlenik zaposlenik, string ReturnUrl)
        {
            if (IsValid(zaposlenik))
            {
                FormsAuthentication.SetAuthCookie(zaposlenik.KorisnickoIme, false);
                return RedirectToAction("Predbiljezbe", "Predbiljezbe");
            }
            else
            {
                ViewBag.Message = "Korisničko ime i zaporka nisu ispravni";
                return View(zaposlenik);
            }
        }

        [AllowAnonymous]
        public ActionResult PrvaRegistracija()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Zaposlenik.Count() == 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Predbiljezba", "Predbiljezba");
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult PrvaRegistracija(Zaposlenik zaposlenik)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (db.Zaposlenik.Count() == 0)
                    {
                        db.Zaposlenik.Add(zaposlenik);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "Registracija prvog zaposlenika " + zaposlenik.Ime + " " + zaposlenik.Prezime + " je uspješno izvršena!";                        
                    }
                    else
                    {
                        ViewBag.Message = "Prva prijava nije moguća jer postoje registrirani zaposlenici.";
                    }                        
                }                
            }

            return View();
        }

        public ActionResult Registracija()
        {

            return View();
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Registracija(Zaposlenik zaposlenik)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (!db.Zaposlenik.Any(x => x.KorisnickoIme == zaposlenik.KorisnickoIme))
                    {
                        db.Zaposlenik.Add(zaposlenik);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Korisničko ime " + zaposlenik.KorisnickoIme + " je zauzeto.";
                        return View();
                    }
                }

                ModelState.Clear();
                ViewBag.Message = "Registracija novog zaposlenika " + zaposlenik.Ime + " " + zaposlenik.Prezime + " je uspješno izvršena!";
            }

            return View();
        }

        public ActionResult UrediZaposlenika()
        {
            Zaposlenik zaposlenik = new Zaposlenik();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                zaposlenik = _db.Zaposlenik.Where(x => x.KorisnickoIme == User.Identity.Name).FirstOrDefault();
                ViewBag.KorisnickoImeZaposlenika = zaposlenik.KorisnickoIme;
            }

            return View(zaposlenik);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult UrediZaposlenika(Zaposlenik zaposlenik)
        {
            Zaposlenik zaposlenikPromjena = new Zaposlenik();

            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    zaposlenikPromjena = db.Zaposlenik.Find(zaposlenik.IdZaposlenik);
                    zaposlenikPromjena.IdZaposlenik = zaposlenik.IdZaposlenik;
                    zaposlenikPromjena.Ime = zaposlenik.Ime;
                    zaposlenikPromjena.Prezime = zaposlenik.Prezime;
                    zaposlenikPromjena.Email = zaposlenik.Email;
                    zaposlenikPromjena.KorisnickoIme = zaposlenik.KorisnickoIme;
                    zaposlenikPromjena.Zaporka = zaposlenik.Zaporka;
                    zaposlenikPromjena.ZaporkaPotvrda = zaposlenik.ZaporkaPotvrda;

                    db.Entry(zaposlenikPromjena).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.Message = "Promjena podataka zaposlenika " + zaposlenik.Ime + " " + zaposlenik.Prezime + " je uspješno izvršena!";
            }

            ViewBag.KorisnickoImeZaposlenika = zaposlenik.KorisnickoIme;
            return View(zaposlenik);
        }

        public ActionResult ObrisiZaposlenika(int? id)
        {
            Zaposlenik zaposlenik = new Zaposlenik();

            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                zaposlenik = _db.Zaposlenik.Find(id);
            }

            return View(zaposlenik);
        }

        [HttpPost]
        [HandleError]
        [ValidateAntiForgeryToken]
        public ActionResult ObrisiZaposlenika(int id)
        {
            Zaposlenik zaposlenik = new Zaposlenik();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                zaposlenik = db.Zaposlenik.Find(id);

                if (zaposlenik.KorisnickoIme != User.Identity.Name)
                {
                    db.Zaposlenik.Remove(zaposlenik);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Nije moguće obrisati trenutno prijavljenog zaposlenika.";
                    return View(zaposlenik);
                }
            }

            return RedirectToAction("Zaposlenici");
        }

        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            return View();
        }


        private bool IsValid(Zaposlenik zaposlenik)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Zaposlenik zap = _db.Zaposlenik.Where(x => x.KorisnickoIme == zaposlenik.KorisnickoIme && x.Zaporka == zaposlenik.Zaporka).FirstOrDefault();
                if (zap != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}