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
    
    public partial class Skidka
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Skidka()
        {
            this.Abonements = new HashSet<Abonement>();
        }
    
        public int ID_skidka { get; set; }
        public Nullable<int> ID_usluga { get; set; }
        public Nullable<decimal> Procent_value { get; set; }
        public Nullable<int> Kolichestvo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Abonement> Abonements { get; set; }
        public virtual Usluga Usluga { get; set; }
    }
}
