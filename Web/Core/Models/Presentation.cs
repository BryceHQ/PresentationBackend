using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core
{
    public class Presentation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Guid { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public int FolderId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Background { get; set; }

        /// <summary>
        /// 背景显示效果
        /// </summary>
        [StringLength(30)]
        public string Duang{ get; set; }

        [Required]
        public string Raw { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public DateTime LastUpdateTime { get; set; }
    }
}