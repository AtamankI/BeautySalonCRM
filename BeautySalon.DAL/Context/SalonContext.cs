using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BeautySalon.DAL.Context
{
    public partial class SalonContext : DbContext
    {
        public SalonContext()
            : base("name=SalonContext")
        {
        }

        public virtual DbSet<Archive> Archives { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<HallType> HallTypes { get; set; }
        public virtual DbSet<Master> Masters { get; set; }
        public virtual DbSet<MasterCategory> MasterCategories { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialCategory> MaterialCategories { get; set; }
        public virtual DbSet<MaterialManufacturer> MaterialManufacturers { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<SalaryType> SalaryTypes { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archive>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Archive>()
                .Property(e => e.Profit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Booking>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.BookingTime)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.ClientLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.ClientName)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<HallType>()
                .Property(e => e.HallType1)
                .IsUnicode(false);

            modelBuilder.Entity<Master>()
                .Property(e => e.MasterLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Master>()
                .Property(e => e.MasterName)
                .IsUnicode(false);

            modelBuilder.Entity<Master>()
                .HasMany(e => e.Archives)
                .WithRequired(e => e.Master)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MasterCategory>()
                .Property(e => e.MasterCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.MaterialName)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Material>()
                .Property(e => e.PriceGram)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Material>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Total)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Material>()
                .Property(e => e.GramAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<MaterialCategory>()
                .Property(e => e.MaterialCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialCategory>()
                .HasMany(e => e.Materials)
                .WithOptional(e => e.MaterialCategory)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<MaterialManufacturer>()
                .Property(e => e.MaterialManufacturerName)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialManufacturer>()
                .HasMany(e => e.Materials)
                .WithOptional(e => e.MaterialManufacturer)
                .HasForeignKey(e => e.ManufacturerId);

            modelBuilder.Entity<PaymentType>()
                .Property(e => e.PaymentType1)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Visits)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceCategory>()
                .Property(e => e.ServiceCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserLogin)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserPassword)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Visit>()
                .Property(e => e.ServicePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.MaterialPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.AdditionalCost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.Profit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visit>()
                .Property(e => e.Tips)
                .HasPrecision(19, 4);
        }
    }
}
