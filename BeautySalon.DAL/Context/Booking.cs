namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        public int BookingId { get; set; }

        [Column(TypeName = "date")]
        public DateTime BookingDate { get; set; }

        public int? MasterId { get; set; }

        public int? ClientId { get; set; }

        [StringLength(150)]
        public string Notes { get; set; }

        public int? ServiceId { get; set; }

        [StringLength(50)]
        public string BookingTime { get; set; }

        public virtual Client Client { get; set; }

        public virtual Master Master { get; set; }

        public virtual Service Service { get; set; }
    }
}
