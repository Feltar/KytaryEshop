using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kytary.Models
{

    /// <summary>
    /// Model uživatelského profilu pro potřeby zpracování žádostí a verifikaci ohledně přihlášení a registrace. 
    /// </summary>
    public class UzivatelModel
    {
        [DisplayName("Uživatelské jméno")]
        [Required(ErrorMessage = "Musíte zadat křestní jméno.")]
        public string UzivatelskeJmeno { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Emailová adresa")]
        [Required(ErrorMessage = "Musíte zadat emailovou adresu.")]
        public string EmailovaAdresa { get; set; }

        [DisplayName("Křestní jméno")]
        [Required(ErrorMessage = "Musíte zadat křestní jméno.")]
        public string KrestniJmeno { get; set; }

        [DisplayName("Příjmení")]
        [Required(ErrorMessage = "Musíte zadat příjmení.")]
        public string Prijmeni { get; set; }

        [DisplayName("Adresa")]
        [Required(ErrorMessage = "Musíte zadat adresu.")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Musíte zadat heslo.")]
        [DisplayName("Heslo")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Heslo musí mít alespoň osm znaků.")]
        public string Heslo { get; set; }


        [DisplayName("Potvrďte heslo")]
        [Required(ErrorMessage = "Musíte zadat potvzení hesla.")]
        [DataType(DataType.Password)]
        [Compare("Heslo", ErrorMessage = "Heslo a potvrzení hesla se musí shodovat.")]
        public string PotvrzeniHesla { get; set; }

        public string ChybovaHlaska = "";

    }
}
