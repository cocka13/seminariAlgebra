using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public enum EnumStatusPredbiljezbe
    {
        [Display(Name = "Neobrađena")] Neobradjena = 0,
        [Display(Name = "Odbijena")] Odbijena = -1,
        [Display(Name = "Prihvaćena")] Prihvacena = 1

    }


    [Table("Seminari")]
    public class Seminar
    {
        [Key]
        public int IdSeminar { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DefaultValue(false)]
        [DisplayName("Objava")]
        public bool Objava { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "2-100 znakova")]
        [DisplayName("Naziv seminara")]
        public string Naziv { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Opis")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.Date, ErrorMessage = "Pogrešan upis datuma")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Datum početka")]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DefaultValue(1)]
        [Range(1, 100, ErrorMessage = "1-100 polaznika")]
        [DisplayName("Maksimalan broj polaznika")]
        public int MaxBrojPolaznika { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DefaultValue(false)]
        [DisplayName("Popunjenost")]
        public bool Popunjen { get; set; }


        public virtual ICollection<Predbiljezba> Predbiljezbe { get; set; }

    }

    [Table("Predbiljezbe")]
    public class Predbiljezba
    {
        [Key]
        public int IdPredbiljezba { get; set; }

        public int IdSeminar { get; set; }
        public Seminar Seminar { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.DateTime, ErrorMessage = "Pogrešan upis datuma")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Datum")]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "1-50 znakova")]
        [DisplayName("Ime")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "1-100 znakova")]
        [DisplayName("Prezime")]
        public string Prezime { get; set; }

        [DisplayName("Adresa")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Neispravan unos email adrese")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage = "max. 25 znakova")]
        [DisplayName("Telefon")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DefaultValue(EnumStatusPredbiljezbe.Odbijena)]
        [DisplayName("Status")]
        public EnumStatusPredbiljezbe StatusPredbiljezbe { get; set; }
    }

    [Table("Zaposlenici")]
    public class Zaposlenik
    {
        [Key]
        public int IdZaposlenik { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "1-50 znakova")]
        [DisplayName("Ime zaposlenika")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "1-100 znakova")]
        [DisplayName("Prezime zaposlenika")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Neispravan unos email adrese")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "1-50 znakova")]
        [DisplayName("Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "1-50 znakova")]
        [DisplayName("Zaporka")]
        public string Zaporka { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [Compare("Zaporka", ErrorMessage = "Nije upisana istovjetna zaporka")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "1-50 znakova")]
        [DisplayName("Potvrda zaporke")]
        public string ZaporkaPotvrda { get; set; }
    }

    [Table("GreskeLog")]
    public class Greske
    {
        [Key]
        public int IdGreske { get; set; }

        public DateTime VrijemeGreske { get; set; }
        public string Poruka { get; set; }
        public string Izvor { get; set; }
        public string Greska { get; set; }
    }
}