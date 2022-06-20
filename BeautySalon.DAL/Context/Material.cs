namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        public int MaterialId { get; set; }

        [StringLength(50)]
        public string MaterialName { get; set; }

        public int Volume { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Column(TypeName = "money")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? PriceGram { get; set; }

        public int? CategoryId { get; set; }

        public int? ManufacturerId { get; set; }

        [StringLength(10)]
        public string Number { get; set; }

        public decimal? Total { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? GramAmount { get; set; }

        public virtual MaterialCategory MaterialCategory { get; set; }

        public virtual MaterialManufacturer MaterialManufacturer { get; set; }
    }
}
