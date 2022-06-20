namespace BeautySalon.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserLogin { get; set; }

        [Required]
        [StringLength(50)]
        public string UserPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string UserEmail { get; set; }
    }
}
