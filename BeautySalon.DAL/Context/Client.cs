namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Bookings = new HashSet<Booking>();
            Visits = new HashSet<Visit>();
        }

        public int ClientId { get; set; }

        [StringLength(50)]
        public string ClientLastName { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? NumberOfVisits { get; set; }

        [StringLength(150)]
        public string Notes { get; set; }

        public int? InArchive { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastVisit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visit> Visits { get; set; }
    }
}
