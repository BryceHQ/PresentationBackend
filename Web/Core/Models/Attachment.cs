using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Http;

namespace Web.Core
{
    public class Attachment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Guid { get; set; }


        /// <summary>
        /// 客户名称。
        /// </summary>
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 附件文件路径（相对于配置的文件上传根目录）。
        /// </summary>
        [StringLength(128)]
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// 附件文件路径（相对于配置的文件上传根目录）。
        /// </summary>
        [StringLength(200)]
        [Required]
        public string FileName { get; set; }

        [NotMapped]
        public string Url
        {
            get
            {
                string baseUrl = GlobalConfiguration.Configuration.VirtualPathRoot;
                char ch = baseUrl[baseUrl.Length - 1];
                if (ch == '/')
                {
                    return baseUrl + FilePath.Replace(@"\", "/") + "/" + this.FileName;
                }
                else
                {
                    return baseUrl + "/" + FilePath.Replace(@"\", "/") + "/" + this.FileName;
                }
            }
        }

        [NotMapped]
        public string FullPath
        {
            get
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.FilePath, this.FileName);
            }
        }

        /// <summary>
        /// 上传用户Id
        /// </summary>
        [Required]
        public string UploadUser { get; set; }

        /// <summary>
        /// 附件上传时间
        /// </summary>
        [Required]
        public DateTime UploadTime { get; set; }

    }
}
