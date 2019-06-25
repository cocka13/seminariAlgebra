using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class ViewModeli
    {

        public class SeminarViewModel
        {
            public int IdSeminar { get; set; }
            public bool Objava { get; set; }
            [DisplayName("Naziv seminara")]
            public string Naziv { get; set; }
            [DisplayName("Opis")]
            public string Opis { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            [DisplayName("Datum početka")]
            public DateTime Datum { get; set; }
            [DisplayName("Broj polaznika")]
            public int MaxBrojPolaznika { get; set; }
            [DisplayName("Popunjenost")]
            public bool Popunjen { get; set; }

            public int UkupanBrojPredbiljezbi { get; set; }
            public int BrojOdobrenihPredbiljezbi { get; set; }
            public int BrojNeobradjenihPredbiljezbi { get; set; }
        }

        public class PredbiljezbaViewModel
        {
            public int IdPredbiljezba { get; set; }
            public int IdSeminar { get; set; }
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy. HH:mm}")]
            [DisplayName("Datum")]
            public DateTime Datum { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Adresa { get; set; }
            public string Email { get; set; }
            public string Telefon { get; set; }
            [DisplayName("Status")]
            public EnumStatusPredbiljezbe StatusPredbiljezbe { get; set; }

            [DisplayName("Naziv seminara")]
            public string NazivSeminara { get; set; }
        }
    }
}