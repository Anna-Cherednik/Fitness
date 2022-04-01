using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Models
{
    public class ClientViewAbonementModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_abonement { get; set; }

        [Display(Name = "Название услуги")]
        public string Nazvanie { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public Nullable<System.DateTime> Data_nachala { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public Nullable<System.DateTime> Data_okonchania { get; set; }

        [Display(Name = "Количество дней")]
        public int? Kolichestvo { get; set; }

        [Display(Name = "Остаток посещений")]
        public int? Ostatok { get; set; }

        [Display(Name = "Размер скидки")]
        public double Procent { get; set; }

        [Display(Name = "Стоимость")]
        public Nullable<double> Cena { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ближайшая свободная дата")]
        public Nullable<System.DateTime> Data_vizita { get; set; }

        [Display(Name = "Состояние")]
        public string Sostojanie { get; set; }
    }

    public class NewViewAbonementModel
    {
        [Display(Name = "Название услуги")]
        public string Nazvanie { get; set; }

        [Display(Name = "Количество дней")]
        public int? Kolichestvo { get; set; }

        [Display(Name = "Стоимость")]
        public Nullable<double> Cena { get; set; }
    }

    public class RaspisanieZanjatijViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public List<string> Zanjaties { get; set; }

        public bool inPeriod { get; set; }
    }

    public class PeriodViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Data_nachala { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data_okonchania { get; set; }
    }

    public class CalendarViewModel
    {
        public PeriodViewModel Period { get; set; }

        public List<RaspisanieZanjatijViewModel> Raspisanie { get; set;}
    }
}