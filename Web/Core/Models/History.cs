using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core
{
    public class History
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Guid { get; set; }

        [Required]
        public int PresentationId { get; set; }

        [Required]
        public string Raw { get; set; }

        [StringLength(128)]
        public string Background { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public DateTime LastUpdateTime { get; set; }
    }
}
