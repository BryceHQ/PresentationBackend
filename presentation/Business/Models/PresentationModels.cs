using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace presentation.Business.Models
{
    public class PresentationContext : DbContext
    {
        public PresentationContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<History> History { get; set; }
    }

    [Table("Presentation")]
    public class Presentation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int UserId { get; set; }
        [MaxLength]
        public string Raw { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    [Table("History")]
    public class History
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PresentationId { get; set; }
        [MaxLength]
        public string Raw { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

}
