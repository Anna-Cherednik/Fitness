﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Abonement> Abonements { get; set; }
        public virtual DbSet<Adress> Adresses { get; set; }
        public virtual DbSet<Dolgnost> Dolgnosts { get; set; }
        public virtual DbSet<Klient> Klients { get; set; }
        public virtual DbSet<Pomeshenie> Pomeshenies { get; set; }
        public virtual DbSet<Poseshenie> Poseshenies { get; set; }
        public virtual DbSet<Raspisanie_zanjatij> Raspisanie_zanjatij { get; set; }
        public virtual DbSet<Skidka> Skidkas { get; set; }
        public virtual DbSet<Sotrudnik> Sotrudniks { get; set; }
        public virtual DbSet<Ulica> Ulicas { get; set; }
        public virtual DbSet<Usluga> Uslugas { get; set; }
        public virtual DbSet<Zarplata_administratoram> Zarplata_administratoram { get; set; }
        public virtual DbSet<Zarplata_instruktoram> Zarplata_instruktoram { get; set; }
    }
}
