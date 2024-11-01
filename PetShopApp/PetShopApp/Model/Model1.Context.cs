﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetShopApp.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TradeEntities : DbContext
    {
        private static TradeEntities _context;
        public TradeEntities()
            : base("name=TradeEntities")
        {
        }

        public static TradeEntities GetContext()
        {
            if (_context == null)
            {
                _context = new TradeEntities();
            }
            return _context;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Importers> Importers { get; set; }
        public virtual DbSet<Manufacturies> Manufacturies { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PickPoints> PickPoints { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductNames> ProductNames { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<Streets> Streets { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
