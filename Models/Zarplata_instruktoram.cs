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
    
    public partial class Zarplata_instruktoram
    {
        public int ID_nachislenia { get; set; }
        public int ID_usluga { get; set; }
        public int ID_poseshenia { get; set; }
        public Nullable<double> Summa { get; set; }
    
        public virtual Poseshenie Poseshenie { get; set; }
        public virtual Usluga Usluga { get; set; }
    }
}
