//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fitness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Abonement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Abonement()
        {
            this.Poseshenies = new HashSet<Poseshenie>();
            this.Zarplata_administratoram = new HashSet<Zarplata_administratoram>();
        }
    
        public int ID_abonement { get; set; }
        public int ID_usluga { get; set; }
        public Nullable<int> ID_skidka { get; set; }
        public int ID_klient { get; set; }
        public Nullable<int> ID_sotrudnik { get; set; }
        public Nullable<System.DateTime> Data_nachala { get; set; }
        public Nullable<System.DateTime> Data_okonchania { get; set; }
        public Nullable<int> Kolichestvo { get; set; }
        public string Sostojanie { get; set; }
        public Nullable<int> Ostatok { get; set; }
        public Nullable<double> Stoimost { get; set; }
    
        public virtual Klient Klient { get; set; }
        public virtual Skidka Skidka { get; set; }
        public virtual Sotrudnik Sotrudnik { get; set; }
        public virtual Usluga Usluga { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Poseshenie> Poseshenies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zarplata_administratoram> Zarplata_administratoram { get; set; }
    }
}
