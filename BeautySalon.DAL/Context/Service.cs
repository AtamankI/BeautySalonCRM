namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Service")]
    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            Bookings = new HashSet<Booking>();
            Visits = new HashSet<Visit>();
        }

        public int ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; }

        public int? ServiceCategoryId { get; set; }

        public int? HallTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? Avaliability { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual HallType HallType { get; set; }

        public virtual ServiceCategory ServiceCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visit> Visits { get; set; }
    }
}
