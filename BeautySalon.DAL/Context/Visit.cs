namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Visit")]
    public partial class Visit
    {
        public int VisitId { get; set; }

        [Column(TypeName = "date")]
        public DateTime VisitDate { get; set; }

        public int? MasterId { get; set; }

        public int? ClientId { get; set; }

        public int ServiceId { get; set; }

        public int? PaymentTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ServicePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaterialPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdditionalCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Profit { get; set; }

        [Column(TypeName = "money")]
        public decimal? Salary { get; set; }

        [Column(TypeName = "money")]
        public decimal? Tips { get; set; }

        public virtual Client Client { get; set; }

        public virtual Master Master { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Service Service { get; set; }
    }
}
