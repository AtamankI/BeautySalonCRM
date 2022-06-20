namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Archive")]
    public partial class Archive
    {
        public int ArchiveId { get; set; }

        [Column(TypeName = "date")]
        public DateTime ArchiveDate { get; set; }

        public int MasterId { get; set; }

        public int? TotalCustomers { get; set; }

        [Column(TypeName = "money")]
        public decimal? Salary { get; set; }

        [Column(TypeName = "money")]
        public decimal? Profit { get; set; }

        public virtual Master Master { get; set; }
    }
}
