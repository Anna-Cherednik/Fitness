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
    
    public partial class Raspisanie_zanjatij
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Raspisanie_zanjatij()
        {
            this.Poseshenies = new HashSet<Poseshenie>();
        }
    
        public int ID_zanjatia { get; set; }
        public int ID_usluga { get; set; }
        public System.DateTime Data { get; set; }
        public Nullable<int> ID_sotrudnik { get; set; }
        public Nullable<int> ID_pomeshenia { get; set; }
    
        public virtual Pomeshenie Pomeshenie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Poseshenie> Poseshenies { get; set; }
        public virtual Sotrudnik Sotrudnik { get; set; }
        public virtual Usluga Usluga { get; set; }
    }
}
