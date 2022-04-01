using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class FitnessViewModel
    {
        public List<UslugaViewModel> Uslugas = new List<UslugaViewModel>();
    }

    public class UslugaViewModel
    {
        public string ImagePath { get; set; }

        [Display(Name = "")]
        public string Nazvanie { get; set; }

        [Display(Name = "Цена")]
        public Nullable<double> Cena_za_poseshenie
        { get; set; }
    }
}