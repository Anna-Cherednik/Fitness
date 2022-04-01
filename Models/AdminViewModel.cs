using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Fitness.Models
{
    public class BonusUslugaViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_usluga { get; set; }

        [Display(Name = "Название услуги")]
        public string Nazvanie { get; set; }

        [Display(Name = "Стоимость")]
        public Nullable<double> Cena_za_poseshenie { get; set; }

        [Display(Name = "Скидки")]
        public virtual List<string> Skidkas { get; set; }
    }

    public class SkidkaViewModel
    {
        [Display(Name = "Название услуги")]
        public string Nazvanie_uslugi { get; set; }

        [Display(Name = "Процент скидки")]
        public double Procent_value { get; set; }

        [Display(Name = "Количество дней")]
        public int Kolichestvo { get; set; }
    }

    public class RaspisanieViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_zanjatia { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата занятия")]
        public System.DateTime Data { get; set; }

        [Display(Name = "Название услуги")]
        public string Usluga { get; set; }

        [Display(Name = "ФИО сотрудника")]
        public string Sotrudnik { get; set; }

        [Display(Name = "Помещение")]
        public String Pomeshenie { get; set; }

        public bool IsVisit { get; set; }
    }

    public class CheckZanjatieViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_zanjatia { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата занятия")]
        public System.DateTime Data { get; set; }

        [Display(Name = "Название услуги")]
        public string Usluga { get; set; }

        public virtual List<VisitedClientViewModel> klients { get; set; }
    }

    public class PosesheniesViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Дата посещения")]
        public System.DateTime Data { get; set; }

        [Display(Name = "Название услуги")]
        public string Usluga { get; set; }

        [Display(Name = "Посетители")]
        public virtual List<string> klients { get; set; }

        [Display(Name = "Количество человек")]
        public int Kolichestvo { get; set; }
    }

    public class VisitedClientViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_abonement { get; set; }

        [Display(Name = "ФИО клиента")]
        public string Klient { get; set; }

        public bool Visit { get; set; }
    }

    public class AdminViewAbonementModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID_abonement { get; set; }

        [Display(Name = "ФИО клиента")]
        public string Klient_FIO { get; set; }

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
        public Nullable<double> Stoimost { get; set; }

        [Display(Name = "Состояние")]
        public string Sostojanie { get; set; }
    }

    public class CreateKlientViewModel
    {
        [Display(Name = "Фамилия Имя Отчество")]
        public string FIO { get; set; }

        [Phone]
        [Display(Name = "Телефон")]
        public string Telephon { get; set; }

        [Display(Name = "Улица")]
        public string Ulica { get; set; }
        [Display(Name = "Номер дома")]
        public int Nomer_doma { get; set; }
        [Display(Name = "Номер квартиры")]
        public int Nomer_kvartiru { get; set; }
    }

    public class EditAbonementViewModel
    {
        [Display(Name = "ФИО клиента")]
        public string Klient_FIO { get; set; }

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

        public string Skidka { get; set; }

        [Display(Name = "Стоимость")]
        public Nullable<double> Stoimost { get; set; }
    }

    public class SotrudnikViewModel
    {
        [Display(Name = "ФИО")]
        public string FIO_sotrudnik { get; set; }

        [Display(Name = "Телефон")]
        public string Telephon { get; set; }

        [Display(Name = "Паспорт")]
        public string Pasport { get; set; }

        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Display(Name = "Должность")]
        public string Dolgnost { get; set; }
    }


    public class ListZarplataViewModel
    {
        public bool AdminFirst { get; set; }

        public DateTime Data_nachala { get; set; }
        public DateTime Data_okonchania { get; set; }

        public List<ZarplataViewModel> Zarplats = new List<ZarplataViewModel>();
    }

    public class ZarplataViewModel
    {
        [Display(Name = "ФИО")]
        public string FIO_sotrudnik { get; set; }

        [Display(Name = "Должность")]
        public string Dolgnost { get; set; }

        [Display(Name = "Начисления")]
        public double? Summa { get; set; }
    }
}